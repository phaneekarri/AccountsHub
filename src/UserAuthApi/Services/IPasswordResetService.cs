using UserAuthApi.Dto;

namespace UserAuthApi.Services;

public interface IPasswordResetService
{
    Task<bool> SendResetTokenAsync(ForgotPasswordRequest request);
    Task<bool> ResetPasswordAsync(ResetPasswordRequest request);
}