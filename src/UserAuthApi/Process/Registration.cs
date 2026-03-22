using UserAuthApi.Dto;
using UserAuthApi.Exceptions;
using UserAuthApi.Services;
using UserAuthEntities;
using UserAuthEntities.InternalUsers;

namespace UserAuthApi.Process;

public class Registration : IRegistration
{
    private readonly ILogger<Registration> _logger;
    private readonly IUserService _userService;

    public Registration(ILogger<Registration> logger, 
     IUserService user
    )
    {
        _logger = logger;
        _userService = user;
        
    }
    public async Task<UserDto> Register(UserLoginModel userLogin)
    {
        if(await _userService.Get(userLogin.UserIdentifierType, userLogin.UserIdentifier!) != null)
         throw new ConflictException("User already exists");
        var user = userLogin.ToUser();      
        await _userService.Create(user);        
        return user.ToUserDto();
    }

    public async Task<UserDto> Register(InternalUserRegisterModel userRegisterModel)
    {
        if( await _userService.Get(userRegisterModel.UserName) != null
         ||   await _userService.Get(UserIdentifierType.Email, userRegisterModel.Email) != null
         || (!string.IsNullOrEmpty(userRegisterModel.Phone) && await _userService.Get(UserIdentifierType.Phone, userRegisterModel.Phone) != null))
         throw new ConflictException("User already exists");
        var userModel = userRegisterModel.ToUser();
        var user = await _userService.Create(userModel);
        var internalUser = await _userService.Create(user, userRegisterModel.PasswordText);
        return internalUser.ToInternalUserDto();
    }
}