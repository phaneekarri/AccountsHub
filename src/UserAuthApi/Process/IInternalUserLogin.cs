using UserAuthApi.Dto;

namespace UserAuthApi.Process;
public interface IInternalUserLogin
{
    Task<UserDto> LogIn(InternalUserLoginModel userLogin);
    Task EnableMFA(Guid userId);

}