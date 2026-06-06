using UserAuthApi.Dto;
using UserAuthEntities;

namespace UserAuthApi.Services;

public interface IOtpService
{
    Task<UserOtp> Create(Guid userId, UserIdentifierType otpReceiver, OtpType otpType = OtpType.Verification);
    Task<UserOtp?> GetRecentOtp(Guid userId, UserIdentifierType otpReceiver, OtpType otpType = OtpType.Verification);
    Task Remove(Guid userId, UserIdentifierType receiver, OtpType otpType = OtpType.Verification);

}
