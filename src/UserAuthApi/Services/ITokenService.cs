using UserAuthEntities;

namespace UserAuthApi;

public interface ITokenService
{
    string GenerateToken(User user);
}
