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
    private readonly ITokenService _tokenService;
    private readonly IOtp _otpProcess;

    private readonly IRegistration _registrationProcess;

    public UserLogin(ILogger<UserLogin> logger, IMapper mapper, 
    IRegistration registrationProcess, ITokenService tokenService,
    IUserService userService, IOtp otpProcess)
    {
        _logger = logger;
        _mapper = mapper;
        _userService = userService;
        _otpProcess = otpProcess;
        _tokenService = tokenService;
        _registrationProcess = registrationProcess;
    }

    public async Task<UserDto> LogIn(UserLoginModel userLogin)
    {
        if (string.IsNullOrEmpty(userLogin.UserIdentifier)) throw new ValidationException("Identifier is required");        
        User? user = await _userService.Get(userLogin.UserIdentifierType, userLogin.UserIdentifier);        
        var userdto = user == null || user.Id == default ? 
            await _registrationProcess.Register(userLogin) 
            : _mapper.Map<UserDto>(user);
        await _otpProcess.Generate(userdto!.Id, userLogin.UserIdentifierType);
        return userdto;
    }

    public async Task ResendOtp(Guid userId, UserIdentifierType otpReceiver)
    {
        User? user = await _userService.Get(userId);
       if(user == null) throw new KeyNotFoundException("User not found");
       await _otpProcess.Generate(user!.Id, otpReceiver);
    }

    public async Task<AuthTokenModel> Authenticate(OtpVerficationModel otp)
    {
       var user = await _userService.Get(otp.Id);      
      if(user == null) throw new ValidationException("Invalid user details");
      if(otp?.Otp == null) throw new ValidationException("Invalid Otp Code");
      if(await _otpProcess.Verify(otp.Id, otp.UserIdentifierType, otp.Otp))
    {
         return _tokenService.GenerateToken(user);
       }
       else throw new ValidationException("Invalid Otp Code");
    }

}
