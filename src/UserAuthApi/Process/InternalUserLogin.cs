using System.Security.Authentication;
using UserAuthApi.Dto;
using UserAuthApi.Services;
using UserAuthEntities;

namespace UserAuthApi.Process;
public class InternalUserLogin(ILogger<InternalUserLogin> logger, IUserService service) : IInternalUserLogin
{
    private readonly ILogger<InternalUserLogin> _logger = logger;
    private readonly IUserService _userService = service;
    
    public async Task EnableMFA(Guid userId)
    {
        await _userService.EnableMFA(userId, MfaMethod.SmsOtp);
    }
    public async Task<UserDto> LogIn(InternalUserLoginModel userLogin)
    {
        User? user = await _userService.Get(userLogin.UserName);
        if(user == null) throw new KeyNotFoundException("User not found");
        
        // Verify password
        bool isValidPassword = await _userService.VerifyPassword(userLogin.UserName, userLogin.PasswordText);
        if (!isValidPassword) throw new AuthenticationException("Incorrect password");
        
        return user.ToInternalUserDto();
    }

}