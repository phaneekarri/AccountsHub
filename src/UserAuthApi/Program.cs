
using FluentValidation;
using FluentValidation.AspNetCore;
using Infra.Notification;
using InfraEntities.Interceptors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
builder.Services.AddValidatorsFromAssemblyContaining<ForgotPasswordRequest>();
builder.Services.AddValidatorsFromAssemblyContaining<ResetPasswordRequest>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProfileRequest>();
builder.Services.AddFluentValidationAutoValidation();

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

var migrationConnectionString = builder.Configuration.GetConnectionString("AuthDatabaseMigration");

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
builder.Services.AddScoped<IPasswordResetService, PasswordResetService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<ITokenRevocationService, TokenRevocationService>();
builder.Services.AddScoped<INotificationSender, EmailNotificationSender>();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
        };
    });
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
      try
      {
          if (!string.IsNullOrWhiteSpace(migrationConnectionString))
          {
              Console.WriteLine("Attempting migrations using migration service account...");
              var migrationOptions = new DbContextOptionsBuilder<AuthDBContext>();
              migrationOptions.UseSqlServer(migrationConnectionString, b => b.MigrationsAssembly("UserAuthMigrations"));
              using var migrationContext = new AuthDBContext(migrationOptions.Options);
              
              // Check if migrations are needed
              var pendingMigrations = migrationContext.Database.GetPendingMigrations();
              if (pendingMigrations.Any())
              {
                  Console.WriteLine($"Found {pendingMigrations.Count()} pending migrations. Applying...");
                  // Retry logic for connection issues
                  int retryCount = 0;
                  const int maxRetries = 3; // Reduced retries for faster failure
                  const int delayMs = 2000;
                  
                  while (retryCount < maxRetries)
                  {
                      try
                      {
                          migrationContext.Database.Migrate();
                          Console.WriteLine("Migrations applied successfully using migration service account.");
                          break;
                      }
                      catch (Exception ex)
                      {
                          retryCount++;
                          string errorMessage = ex.Message.ToLower();
                          
                          // If the error indicates tables already exist, consider it successful
                          if (errorMessage.Contains("already an object named") || 
                              errorMessage.Contains("already exists"))
                          {
                              Console.WriteLine("Migration objects already exist in database. Continuing with startup.");
                              break;
                          }
                          
                          if (retryCount < maxRetries)
                          {
                              Console.WriteLine($"Retry {retryCount}/{maxRetries}: Migration failed - {ex.Message}");
                              System.Threading.Thread.Sleep(delayMs);
                          }
                          else
                          {
                              Console.WriteLine($"Migration failed after {maxRetries} retries: {ex.Message}");
                              Console.WriteLine("Continuing with application startup despite migration failure.");
                              break;
                          }
                      }
                  }
              }
              else
              {
                  Console.WriteLine("No pending migrations found. Database is up to date.");
              }
          }
          else
          {
              Console.WriteLine("Migration connection string not provided, using main context...");
              var context = scope.ServiceProvider.GetRequiredService<AuthDBContext>();
              context.Database.Migrate();
              Console.WriteLine("Migrations applied successfully using main context.");
          }
      }
      catch (Exception ex)
      {
          Console.WriteLine($"Migration failed: {ex.Message}");
          Console.WriteLine($"Stack trace: {ex.StackTrace}");
          throw;
      }
    }  

}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapSignInEndpoints();
app.MapOtpEndpoints();
app.MapAuthenticationEndpoints();
app.MapMFAEndpoints();
app.MapPasswordResetEndpoints();
app.MapProfileEndpoints();
app.Run();


