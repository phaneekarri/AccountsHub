using UserAuthEntities;

namespace UserAuthApi.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}
