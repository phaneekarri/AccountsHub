using System.Security.Authentication;
using AutoMapper;
using UserAuthApi.Dto;
using UserAuthApi.Services;
using UserAuthEntities.InternalUsers;

namespace UserAuthApi.Process;
public class InternalUserLogin(ILogger<InternalUserLogin> logger, IUserService service, IMapper mapper) : IInternalUserLogin
{
    private readonly ILogger<InternalUserLogin> _logger = logger;
    private readonly IMapper _mapper = mapper;
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
            return _mapper.Map<InternalUserDto>(user);
        }
        else throw new AuthenticationException("Incorrect Password");
        
    }

}