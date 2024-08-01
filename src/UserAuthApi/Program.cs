
using FluentValidation;
using FluentValidation.AspNetCore;
using InfraEntities.Interceptors;
using Microsoft.EntityFrameworkCore;
using UserAuthApi;
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

builder.Services.AddSingleton<ITokenService, JwtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOtpService, OtpService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapPost("/SeedDB", (AuthDBContext context)=>
    {
      context.Database.EnsureCreated()  ;
      return Results.Ok();
    });
}
app.UseHttpsRedirection();

app.MapPost("/SignIn", async (UserLoginModel userlogin, IValidator<UserLoginModel> validator, IUserService user) =>
{
  var validation = await validator.ValidateAsync(userlogin);
  if(!validation.IsValid) return Results.BadRequest(validation.Errors.Select(e => e.ErrorMessage));
  var userId = await user.LogIn(userlogin);
  return Results.Ok(userId);  
})
.WithName("SignIn")
.WithOpenApi();

app.MapPost("/Verify-Otp", 
async (OtpVerficationModel userlogin, 
  IValidator<OtpVerficationModel> validator,
   IOtpService otp, ITokenService jwt) =>
{
  var validation = await validator.ValidateAsync(userlogin);
  if(!validation.IsValid) Results.BadRequest(validation.Errors.Select(e => e.ErrorMessage));
  var user =  await otp.Verify(userlogin);  
  if(user == null) return Results.BadRequest("Invalid otp");  
  string token = jwt.GenerateToken(user);
  return token != default ? Results.Ok(token) : Results.Json(new { message = "Token generation failed"}, statusCode: 500);//, "Token generation failed");
})
.WithName("Verify-Otp")
.WithOpenApi();



app.Run();


