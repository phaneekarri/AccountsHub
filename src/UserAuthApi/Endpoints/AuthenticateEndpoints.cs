using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UserAuthApi.Dto;
using UserAuthApi.Process;
using UserAuthApi.Services;
using UserAuthApi.Settings;
using UserAuthEntities;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this WebApplication app)
    {
        app.MapPost("/auth/user", 
        async (InternalUserLoginModel userLoginModel, 
        IValidator<InternalUserLoginModel> validator, 
        IInternalUserLogin process)=>
        {
            var validation = await validator.ValidateAsync(userLoginModel);
            if(!validation.IsValid) return Results.BadRequest(validation.Errors.Select(e => e.ErrorMessage));
            var userId = await process.LogIn(userLoginModel);
            return Results.Ok(userId);  
        })
        .WithName("Authenticate-InternalUser")
        .WithOpenApi()
        .Produces<InternalUserDto>();

        app.MapPost("/auth/Otp", async (OtpVerficationModel userlogin, 
        IValidator<OtpVerficationModel> validator, 
        IUserLogin process)=>
        {
            var validation = await validator.ValidateAsync(userlogin);
            if(!validation.IsValid) Results.BadRequest(validation.Errors.Select(e => e.ErrorMessage));
            var token =  await process.Authenticate(userlogin);
            return token != default 
            ? Results.Ok(token) 
            : Results.Json(new { message = "Token generation failed"}, statusCode: 500);
        })
        .WithName("Authenticate-Otp")
        .WithOpenApi();

        app.MapGet("/auth/google/callback", async (
            [FromQuery] string code,
            [FromServices] GoogleAuthProvider authProvider,
            [FromServices] ITokenService token,
            [FromServices] IUserLogin process,
            CancellationToken cancellation) =>
        {
            if (string.IsNullOrEmpty(code))
                return Results.BadRequest("Authorization code is missing");
            var userIdentity = await authProvider.AuthenticateAsync(code, cancellation);
            if (userIdentity != null)
            {
                var user = await process.LogIn(
                    new UserLoginModel(UserIdentifierType.Email, userIdentity.Email));
                return Results.Ok(token.GenerateToken(new User
                {
                    Id = user.Id,
                    Email = user.Email
                }));
            }
            return Results.BadRequest();
        })
        .WithName("Authenticate-GoogleUser");
    }

    public static void RegisterGoogleAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<GoogleAuthProvider>();
        builder.Services.AddOptions<GoogleAuthSettings>()
            .Bind(builder.Configuration.GetSection("GoolgeAuth"))
            .Validate(options => !string.IsNullOrEmpty(options.Secret), "Google Auth Client Secret is required")
            .Validate(options => !string.IsNullOrEmpty(options.ClientId), "Google Auth ClientId is required")
            .Validate(options => !string.IsNullOrEmpty(options.RedirectURI), "Google Auth Redirect Uri is required")
            .Validate(Options => !string.IsNullOrEmpty(Options.TokenEndpointURL), "Google Auth Token Endpoint is required")
            .Validate(options => !string.IsNullOrWhiteSpace(options.UserInfoEndpointURL), "Google Auth User Info Endpoint is required");

    }
}