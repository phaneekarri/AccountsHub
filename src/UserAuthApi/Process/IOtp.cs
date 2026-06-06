using UserAuthApi.Dto;
using UserAuthEntities;

namespace UserAuthApi.Process;
public interface IOtp
{
   Task<OtpModel> Generate(Guid userId, UserIdentifierType otpReceiver, OtpType otpType = OtpType.Verification);
   Task<bool> Verify(Guid userId, UserIdentifierType receiver, string otpCode, OtpType otpType = OtpType.Verification);

}