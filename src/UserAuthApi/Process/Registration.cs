using AutoMapper;
using UserAuthApi.Exceptions;
using UserAuthApi.Services;
using UserAuthEntities;

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
    public async Task<User> Register(UserLoginModel userLogin)
    {
        if(await _userService.Get(userLogin.UserIdentifierType, userLogin.UserIdentifier!) != null)
         throw new ConflictException("User already exists");
        var user = _mapper.Map<User>(userLogin);      
        await _userService.Create(user);
        return user;
    }
}