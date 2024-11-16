using UserAuthEntities;

namespace UserAuthApi.Services;

public interface ITokenService
{
    AuthTokenModel GenerateToken(User user);
}
