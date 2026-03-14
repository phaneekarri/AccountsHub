using FluentValidation;
using UserAuthApi.Dto;
using UserAuthApi.Process;

public static class SignInEndpoints
{
   public static void MapSignInEndpoints(this WebApplication app)
   {
        app.MapPost("/otp/SignIn",
         async (UserLoginModel userlogin, 
            IValidator<UserLoginModel> validator, 
            IUserLogin process) =>
        {
            var validation = await validator.ValidateAsync(userlogin);
            if(!validation.IsValid) 
            return Results.BadRequest(validation.Errors.Select(e => e.ErrorMessage));
            var userId = await process.LogIn(userlogin);
            return Results.Ok(userId);  
        })
        .WithName("OTP_SignIn")
        .WithOpenApi();

        app.MapPost("/SignIn",
         async (InternalUserLoginModel userLoginModel, 
            IValidator<InternalUserLoginModel> validator, 
            IInternalUserLogin process) =>
        {
            var validation = await validator.ValidateAsync(userLoginModel);
            if(!validation.IsValid) 
            return Results.BadRequest(validation.Errors.Select(e => e.ErrorMessage));
            var userId = await process.LogIn(userLoginModel);
            return Results.Ok(userId);  
        })
        .WithName("SignIn")
        .WithOpenApi()
        .Produces<InternalUserDto>();

        app.MapPost("/SignUp", 
        async (InternalUserRegisterModel userRegisterModel,
            IValidator<InternalUserRegisterModel> validator, 
            IRegistration process) =>
        {
            var validation = await validator.ValidateAsync(userRegisterModel);
            if(!validation.IsValid) 
            return Results.BadRequest(validation.Errors.Select(e => e.ErrorMessage));
            var user = await process.Register(userRegisterModel);
            return Results.Ok(user);  
        })
        .WithName("SignUp")
        .WithOpenApi()
        .Produces<InternalUserDto>();
   }

   public static void RegisterSignInProcess(this WebApplicationBuilder builder )
   {
        builder.Services.AddScoped<IUserLogin, UserLogin>();
        builder.Services.AddScoped<IInternalUserLogin, InternalUserLogin>();
        builder.Services.AddScoped<IRegistration, Registration>();
   }
}