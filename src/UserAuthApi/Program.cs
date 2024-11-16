
using FluentValidation;
using FluentValidation.AspNetCore;
using InfraEntities.Interceptors;
using Microsoft.EntityFrameworkCore;
using UserAuthApi;
using UserAuthApi.Process;
using UserAuthApi.Services;
using UserAuthApi.Settings;
using UserAuthEntities;

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
            entry.Entity.CreatedAt = DateTime.Now;       
            }));
},  ServiceLifetime.Singleton);

builder.Services.AddOptions<OtpSettings>()
                .Bind(builder.Configuration.GetSection("Otp"))                
                .Validate(options => options.OtpCodeMax > options.OtpCodeMin,
                           "OtpCodeMax must be greater than OtpCodeMax");

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

app.MapPost("/SignIn", async (UserLoginModel userlogin, IValidator<UserLoginModel> validator, IUserLogin process) =>
{
  var validation = await validator.ValidateAsync(userlogin);
  if(!validation.IsValid) return Results.BadRequest(validation.Errors.Select(e => e.ErrorMessage));
  var userId = await process.LogIn(userlogin);
  return Results.Ok(userId);  
})
.WithName("SignIn")
.WithOpenApi();

app.MapPost("/Verify-Otp", 
async (OtpVerficationModel userlogin, 
  IValidator<OtpVerficationModel> validator,
  IUserLogin process) =>
{
  var validation = await validator.ValidateAsync(userlogin);
  if(!validation.IsValid) Results.BadRequest(validation.Errors.Select(e => e.ErrorMessage));
  var token =  await process.Authenticate(userlogin);
  return token != default ? Results.Ok(token) : Results.Json(new { message = "Token generation failed"}, statusCode: 500);//, "Token generation failed");
})
.WithName("Verify-Otp")
.WithOpenApi();


app.Run();


