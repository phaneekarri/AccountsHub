
using FluentValidation;
using FluentValidation.AspNetCore;
using InfraEntities.Interceptors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using UserAuthApi.Dto;
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

builder.Services.AddOptions<JwtSettings>()
    .Bind(builder.Configuration.GetSection("Jwt"))
    .Validate(options => !string.IsNullOrEmpty(options.Secret), "Jwt Secret is required")
    .Validate(options => !string.IsNullOrEmpty(options.Issuer), "Jwt Issuer is required")
    .Validate(options => !string.IsNullOrEmpty(options.Audience), "Jwt Audience is required");


builder.RegisterSignInProcess();
builder.RegisterGoogleAuth();
builder.RegisterOtp();

builder.Services.AddSingleton<ITokenService, JwtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
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
app.UseExceptionHandler(appBuilder => {
  appBuilder.Run(async context =>
  {
      var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = exception switch
      {
          KeyNotFoundException => 404,
          UnauthorizedAccessException => 401,
          _ => 500
      };
      var errorResponse = new 
      {
          Message = exception?.Message ?? "An unexpected error occurred.",
          ErrorCode = exception?.GetType().Name,
      };
      await context.Response.WriteAsJsonAsync(errorResponse);
  });
});
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
app.UseAuthentication();
app.UseAuthorization();

app.MapSignInEndpoints();
app.MapOtpEndpoints();
app.MapAuthenticationEndpoints();
app.MapMFAEndpoints();
app.Run();


