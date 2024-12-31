using System.Security.Authentication;
using AutoMapper;
using UserAuthApi.Dto;
using UserAuthApi.Exceptions;
using UserAuthApi.Services;
using UserAuthEntities;
using UserAuthEntities.InternalUsers;

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

    public async Task<UserDto> LogIn(UserLoginModel userLogin)
    {
        if (string.IsNullOrEmpty(userLogin.UserIdentifier)) throw new ValidationException("Identifier is required");        
        User? user = await _userService.Get(userLogin.UserIdentifierType, userLogin.UserIdentifier);        
        var userdto = user == null || user.Id == default ? 
            await _registrationProcess.Register(userLogin) 
            : _mapper.Map<UserDto>(user);
        await _otpService.Generate(userdto!.Id, userLogin.UserIdentifierType);
        return userdto;
    }

    public async Task<UserDto> LogIn(InternalUserLoginModel userLogin)
    {
        InternalUser? user = await _userService.Get(userLogin.UserName);
        if(user?.User == null || user.Id == default || user.UserId == default ) throw new KeyNotFoundException("User not found");
        if(user.TryAuthenticate(userLogin.PasswordText))
        {
            return _mapper.Map<InternalUserDto>(user);
        }
        else throw new AuthenticationException("Incorrect Password");
        
    }

    public async Task ResendOtp(Guid userId, UserIdentifierType otpReceiver)
    {
        User? user = await _userService.Get(userId);
       if(user == null) throw new KeyNotFoundException("User not found");
       await _otpService.Generate(user!.Id, otpReceiver);
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

    public async Task EnableMFA(Guid userId)
    {
        await _userService.EnableMFA(userId);
    }

}
