using UserAuthApi.Dto;
using UserAuthEntities;

namespace UserAuthApi.Process;

public interface IRegistration
{
    Task<User> Register(UserLoginModel userLogin);
}
