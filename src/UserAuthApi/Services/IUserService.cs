using UserAuthEntities;

namespace UserAuthApi;

public interface IUserService
{
    Task<User> Register(UserLoginModel userLogin);
    Task<Guid> LogIn(UserLoginModel userLogin);
    Task <string> GetToken(Guid userId);
}
