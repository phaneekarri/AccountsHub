
using AutoMapper;
using UserAuthApi.Exceptions;
using UserAuthEntities;

namespace UserAuthApi;

public class UserService : BaseService<UserService, AuthDBContext>, IUserService
{
    private readonly IOtpService otp;
    private readonly ITokenService token;
    public UserService(ILogger<UserService> logger, IMapper mapper, 
       AuthDBContext context, IOtpService otpService, ITokenService tokenService) 
    : base(logger, mapper, context)
    {
        otp = otpService;
        token = tokenService;
    }

    public async Task<User> Register(UserLoginModel userLogin)
    {
        var user = GetUser(userLogin);
        if(user != null) throw new ConflictException("User already exists");
        user = Mapper.Map<User>(userLogin);      
        Context.Users.Add( user);
        await Context.SaveChangesAsync();
        return user;
    }

    public async Task<Guid> LogIn(UserLoginModel userLogin)
    {
        User? user = GetUser(userLogin);
        if(user == null || user.Id == default) 
            user = await Register(userLogin);         
        await otp.Generate(userLogin, user.Id);
        return user.Id;
    }

    public async Task <string> GetToken(Guid userId)
    {
        User? user = await Context.Users.FindAsync(userId);
        return (user == null) ? string.Empty : token.GenerateToken(user);
    }

    private User? GetUser(UserLoginModel userLogin)
    {
        return Context.Users.SingleOrDefault(x => (userLogin.UserIdentifierType == UserIdentifierType.Email &&  x.Email == userLogin.UserIdentifier)
         || (userLogin.UserIdentifierType == UserIdentifierType.Phone &&  x.Phone  == userLogin.UserIdentifier));
    }
}
