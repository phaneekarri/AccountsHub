using System.Security.Authentication;
using UserAuthApi.Dto;
using UserAuthApi.Services;
using UserAuthEntities.InternalUsers;

namespace UserAuthApi.Process;
public class InternalUserLogin(ILogger<InternalUserLogin> logger, IUserService service) : IInternalUserLogin
{
    private readonly ILogger<InternalUserLogin> _logger = logger;
    private readonly IUserService _userService = service;
    
    public async Task EnableMFA(Guid userId)
    {
        await _userService.EnableMFA(userId);
    }
    public async Task<UserDto> LogIn(InternalUserLoginModel userLogin)
    {
        InternalUser? user = await _userService.Get(userLogin.UserName);
        if(user?.User == null || user.Id == default || user.UserId == default ) throw new KeyNotFoundException("User not found");
        if(user.TryAuthenticate(userLogin.PasswordText))
        {
            return user.ToInternalUserDto();
        }
        else throw new AuthenticationException("Incorrect Password");
        
    }

}