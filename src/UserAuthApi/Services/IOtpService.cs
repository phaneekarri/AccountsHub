using UserAuthEntities;

namespace UserAuthApi;

public interface IOtpService
{
    Task<OtpModel> Generate(UserLoginModel user, Guid userId);

    Task<User> Verify(OtpVerficationModel userOtp);

}
