using AutoMapper;
using UserAuthApi.Dto;
using UserAuthApi.Exceptions;
using UserAuthApi.Services;
using UserAuthEntities;

namespace UserAuthApi.Process;
public class UserLogin : IUserLogin
{
    private readonly ILogger<UserLogin> _logger;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IOtpService _otpService;
    private readonly ITokenService _tokenService;
    private readonly IRegistration _registrationProcess;

    public UserLogin(ILogger<UserLogin> logger, IMapper mapper, 
    IRegistration registrationProcess, ITokenService tokenService,
    IUserService userService, IOtpService otpService)
    {
        _logger = logger;
        _mapper = mapper;
        _userService = userService;
        _otpService = otpService;
        _tokenService = tokenService;
        _registrationProcess = registrationProcess;
    }

    public async Task<Guid> LogIn(UserLoginModel userLogin)
    {
        User? user = null;
        if(!await TryGetUser(userLogin, user))
            user = await _registrationProcess.Register(userLogin);
        await _otpService.Generate(userLogin, user!.Id);
        return user.Id;
    }

    public async Task ResendOtp(UserLoginModel userLogin)
    {
        User? user = null;
       if(!await TryGetUser(userLogin, user)) throw new ValidationException("Invalid user details");
       await _otpService.Generate(userLogin, user!.Id);
    }

    public async Task<AuthTokenModel> Authenticate(OtpVerficationModel otp)
    {
       var user = await _userService.Get(otp.Id);      
      if(user == null) throw new ValidationException("Invalid user details");
       if(await _otpService.Verify(otp))
       {
         return _tokenService.GenerateToken(user);
       }
       else throw new ValidationException("Invalid Otp Code");
    }

    private async Task<bool> TryGetUser(UserLoginModel userLogin, User? user)
    {
        if (string.IsNullOrEmpty(userLogin.UserIdentifier)) throw new ValidationException("Identifier is required");
         user = await _userService.Get(userLogin.UserIdentifierType, userLogin.UserIdentifier);
        return user == null || user.Id == default;
    }


}
