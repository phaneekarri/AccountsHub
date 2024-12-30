using UserAuthApi.Dto;
using UserAuthEntities;

namespace UserAuthApi.Services;

public interface IOtpService
{
    Task<OtpModel> Generate(Guid userId, UserIdentifierType otpReceiver);

    Task<bool> Verify(OtpVerficationModel userOtp);

}
