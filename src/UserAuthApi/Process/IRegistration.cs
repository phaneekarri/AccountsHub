using UserAuthApi.Dto;
using UserAuthEntities;
using UserAuthEntities.InternalUsers;

namespace UserAuthApi.Process;

public interface IRegistration
{
    Task<UserDto> Register(UserLoginModel userLogin);
    Task<UserDto> Register(InternalUserRegisterModel userRegisterModel);
}
