using FluentValidation;
using UserAuthApi.Dto;
using UserAuthApi.Services;

public static class PasswordResetEndpoints
{
    public static void MapPasswordResetEndpoints(this WebApplication app)
    {
        var authGroup = app.MapGroup("/auth");

        authGroup.MapPost("/forgot-password", async (
            ForgotPasswordRequest request,
            IValidator<ForgotPasswordRequest> validator,
            IPasswordResetService service) =>
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.Problem(
                    title: "Validation Error",
                    detail: string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)),
                    statusCode: 400);

            var success = await service.SendResetTokenAsync(request);
            return success
                ? Results.Ok(new { message = "Reset token sent" })
                : Results.Problem(title: "Failed to send reset token", statusCode: 500);
        })
        .WithName("ForgotPassword")
        .WithOpenApi();

        authGroup.MapPost("/reset-password", async (
            ResetPasswordRequest request,
            IValidator<ResetPasswordRequest> validator,
            IPasswordResetService service) =>
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.Problem(
                    title: "Validation Error",
                    detail: string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)),
                    statusCode: 400);

            var success = await service.ResetPasswordAsync(request);
            return success
                ? Results.Ok(new { message = "Password reset successfully" })
                : Results.Problem(title: "Invalid or expired token", statusCode: 400);
        })
        .WithName("ResetPassword")
        .WithOpenApi();

        authGroup.MapPost("/logout", async (
            HttpContext context,
            ITokenRevocationService service) =>
        {
            var authHeader = context.Request.Headers.Authorization.ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Results.Problem(title: "Invalid token", statusCode: 401);

            var token = authHeader.Substring("Bearer ".Length);
            var success = await service.RevokeTokenAsync(token);
            return success
                ? Results.Ok(new { message = "Logged out" })
                : Results.Problem(title: "Token not found", statusCode: 400);
        })
        .RequireAuthorization()
        .WithName("Logout")
        .WithOpenApi();
    }
}