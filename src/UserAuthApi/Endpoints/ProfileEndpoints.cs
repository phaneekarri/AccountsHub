using FluentValidation;
using UserAuthApi.Dto;
using UserAuthApi.Services;

public static class ProfileEndpoints
{
    public static void MapProfileEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("/user");

        userGroup.MapGet("/profile", async (HttpContext context, IProfileService service) =>
        {
            var userIdClaim = context.User.FindFirst("nameid")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                return Results.Problem(title: "Invalid user", statusCode: 401);

            var profile = await service.GetProfileAsync(userId);
            return profile != null
                ? Results.Ok(profile)
                : Results.Problem(title: "User not found", statusCode: 404);
        })
        .RequireAuthorization()
        .WithName("GetProfile")
        .WithOpenApi()
        .Produces<ProfileDto>();

        userGroup.MapPut("/profile", async (
            UpdateProfileRequest request,
            HttpContext context,
            IValidator<UpdateProfileRequest> validator,
            IProfileService service) =>
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.Problem(
                    title: "Validation Error",
                    detail: string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)),
                    statusCode: 400);

            var userIdClaim = context.User.FindFirst("nameid")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                return Results.Problem(title: "Invalid user", statusCode: 401);

            var success = await service.UpdateProfileAsync(userId, request);
            return success
                ? Results.Ok(new { message = "Profile updated" })
                : Results.Problem(title: "Update failed", statusCode: 500);
        })
        .RequireAuthorization()
        .WithName("UpdateProfile")
        .WithOpenApi();
    }
}