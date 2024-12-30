using AutoMapper;
using UserAuthApi.Dto;
using UserAuthApi.Exceptions;
using UserAuthApi.Services;
using UserAuthEntities;
using UserAuthEntities.InternalUsers;

namespace UserAuthApi.Process;

public class Registration : IRegistration
{
    private readonly ILogger<Registration> _logger;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public Registration(ILogger<Registration> logger , IMapper mapper, 
     IUserService user
    )
    {
        _logger = logger;
        _mapper = mapper;
        _userService = user;
        
    }
    public async Task<UserDto> Register(UserLoginModel userLogin)
    {
        if(await _userService.Get(userLogin.UserIdentifierType, userLogin.UserIdentifier!) != null)
         throw new ConflictException("User already exists");
        var user = _mapper.Map<User>(userLogin);      
        await _userService.Create(user);        
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> Register(InternalUserRegisterModel userRegisterModel)
    {
        if( await _userService.Get(userRegisterModel.UserName) != null
         ||   await _userService.Get(UserIdentifierType.Email, userRegisterModel.Email) != null
         || (!string.IsNullOrEmpty(userRegisterModel.Phone) && await _userService.Get(UserIdentifierType.Phone, userRegisterModel.Phone) != null))
         throw new ConflictException("User already exists");
        var userModel = _mapper.Map<User>(userRegisterModel);
        var user = await _userService.Create(userModel);
        var internalUser = await _userService.Create(user, userRegisterModel.PasswordText);
        return _mapper.Map<InternalUserDto>(internalUser);
    }
}