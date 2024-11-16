namespace UserAuthApi.Process;

public interface IUserLogin
{

    Task<Guid> LogIn(UserLoginModel userLogin);
    Task ResendOtp(UserLoginModel userLogin);
    Task<AuthTokenModel> Authenticate(OtpVerficationModel otp);

}