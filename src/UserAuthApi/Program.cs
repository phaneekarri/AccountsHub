
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.AspNetCore;
using InfraEntities.Interceptors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthApi.Dto;
using UserAuthApi.Dto.Validators;
using UserAuthApi.Process;
using UserAuthApi.Services;
using UserAuthApi.Settings;
using UserAuthEntities;
using UserAuthEntities.Interfaces;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddValidatorsFromAssemblyContaining<UserLoginModel>();
builder.Services.AddValidatorsFromAssemblyContaining<OtpVerficationModel>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<AuthDBContext>(options =>
{
    var connstring = builder.Configuration.GetConnectionString("AuthDatabase");
      options.UseSqlServer(connstring)
    .AddInterceptors( 
            new CommonInterceptor<ICreated>( (entry) => {
             if (entry.State == EntityState.Added)
            entry.Entity.CreatedAt = DateTime.UtcNow;       
            }));
},  ServiceLifetime.Singleton);

builder.Services.AddOptions<OtpSettings>()
                .Bind(builder.Configuration.GetSection("Otp"))                
                .Validate(options => options.OtpCodeMax > options.OtpCodeMin,
                           "OtpCodeMax must be greater than OtpCodeMin");

builder.Services.AddOptions<JwtSettings>()
    .Bind(builder.Configuration.GetSection("Jwt"))
    .Validate(options => !string.IsNullOrEmpty(options.Secret), "Jwt Secret is required")
    .Validate(options => !string.IsNullOrEmpty(options.Issuer), "Jwt Issuer is required")
    .Validate(options => !string.IsNullOrEmpty(options.Audience), "Jwt Audience is required");

builder.Services.AddScoped<IUserLogin, UserLogin>();
builder.Services.AddScoped<IRegistration, Registration>();

builder.Services.AddSingleton<ITokenService, JwtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOtpService, OtpService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
   options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using(var scope = app.Services.CreateScope() )
    {
      var context = scope.ServiceProvider.GetRequiredService<AuthDBContext>();
      context.Database.Migrate();
    }  

}
app.UseHttpsRedirection();

app.MapPost("/otp/SignIn", async (UserLoginModel userlogin, IValidator<UserLoginModel> validator, IUserLogin process) 
=>
{
  var validation = await validator.ValidateAsync(userlogin);
  if(!validation.IsValid) return Results.BadRequest(validation.Errors.Select(e => e.ErrorMessage));
  var userId = await process.LogIn(userlogin);
  return Results.Ok(userId);  
})
.WithName("OTP_SignIn")
.WithOpenApi();

app.MapPost("/SignUp", async (InternalUserRegisterModel userRegisterModel, IValidator<InternalUserRegisterModel> validator, IRegistration process) 
=>
{
  var validation = await validator.ValidateAsync(userRegisterModel);
  if(!validation.IsValid) return Results.BadRequest(validation.Errors.Select(e => e.ErrorMessage));
  var user = await process.Register(userRegisterModel);
  return Results.Ok(user);  
})
.WithName("SignUp")
.WithOpenApi()
.Produces<InternalUserDto>();


app.MapPost("/SignIn", async (InternalUserLoginModel userLoginModel, IValidator<InternalUserLoginModel> validator, IUserLogin process) 
=>
{
  var validation = await validator.ValidateAsync(userLoginModel);
  if(!validation.IsValid) return Results.BadRequest(validation.Errors.Select(e => e.ErrorMessage));
  var userId = await process.LogIn(userLoginModel);
  return Results.Ok(userId);  
})
.WithName("SignIn")
.WithOpenApi()
.Produces<InternalUserDto>();

app.MapPost("/EnableMFA", async([FromBody] string userId, IUserLogin process)
=>
{
  if(!Guid.TryParse( userId, out Guid userGuid)) return Results.BadRequest("User is invalid");
  await process.EnableMFA(userGuid);
  return Results.Ok();
})
.WithName("EnableMFA")
.WithOpenApi();

app.MapPost("/SendOtp", async(SendOtpModel otpModel , IValidator<SendOtpModel> validator, IUserLogin process) 
=>
{
  var validation = await validator.ValidateAsync(otpModel);
  if(!validation.IsValid) return Results.BadRequest(validation.Errors.Select(e => e.ErrorMessage));
  await process.ResendOtp(otpModel.Id, otpModel.Receiver);
  return Results.Ok();
})
.WithName("SendOtp")
.WithOpenApi();

app.MapPost("/Verify-Otp", async (OtpVerficationModel userlogin, IValidator<OtpVerficationModel> validator, IUserLogin process)
=>
{
  var validation = await validator.ValidateAsync(userlogin);
  if(!validation.IsValid) Results.BadRequest(validation.Errors.Select(e => e.ErrorMessage));
  var token =  await process.Authenticate(userlogin);
  return token != default ? Results.Ok(token) : Results.Json(new { message = "Token generation failed"}, statusCode: 500);//, "Token generation failed");
})
.WithName("Verify-Otp")
.WithOpenApi();


app.Run();


