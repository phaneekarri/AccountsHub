using UserAuthEntities;

namespace UserAuthApi.Services;

public interface IOtpService
{
    Task<OtpModel> Generate(UserLoginModel user, Guid userId);

    Task<User> Verify(OtpVerficationModel userOtp);

}
