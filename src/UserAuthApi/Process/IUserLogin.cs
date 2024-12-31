using UserAuthApi.Dto;
using UserAuthEntities;

namespace UserAuthApi.Process;

public interface IUserLogin
{
    Task<UserDto> LogIn(UserLoginModel userLogin);
    Task ResendOtp(Guid userId, UserIdentifierType otpReceiver);
    Task<AuthTokenModel> Authenticate(OtpVerficationModel otp);

}
