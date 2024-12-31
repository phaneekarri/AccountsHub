using UserAuthApi.Dto;
using UserAuthEntities;

namespace UserAuthApi.Services;

public interface IOtpService
{
    Task<UserOtp> Create(Guid userId, UserIdentifierType otpReceiver);
    Task<UserOtp?> GetRecentOtp(Guid userId, UserIdentifierType otpReceiver);
    Task Remove(Guid userId, UserIdentifierType receiver);

}
