using FluentValidation;
using UserAuthApi.Dto;
using UserAuthApi.Process;
using UserAuthApi.Services;
using UserAuthApi.Settings;

public static class OtpEndpoints
{
    public static void MapOtpEndpoints(this WebApplication app)
    {
        app.MapPost("/SendOtp", async(SendOtpModel otpModel , 
        IValidator<SendOtpModel> validator, 
        IUserLogin process) =>
        { 
            var validation = await validator.ValidateAsync(otpModel);
            if(!validation.IsValid) return Results.BadRequest(validation.Errors.Select(e => e.ErrorMessage));
            await process.ResendOtp(otpModel.Id, otpModel.Receiver);
            return Results.Ok();
        })
        .WithName("SendOtp")
        .WithOpenApi();
    }

    public static void RegisterOtp(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<OtpSettings>()
                .Bind(builder.Configuration.GetSection("Otp"))                
                .Validate(options => options.OtpCodeMax > options.OtpCodeMin,
                           "OtpCodeMax must be greater than OtpCodeMin");
        builder.Services.AddScoped<IOtpService, OtpService>();
        builder.Services.AddScoped<IOtp, Otp>();
    }
}