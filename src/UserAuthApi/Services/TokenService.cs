using UserAuthEntities;

namespace UserAuthApi.Services;

public class TokenService(JwtService jwt): ITokenService
{
  private readonly JwtService Jwt =jwt;

    public string GenerateToken(User user)
    {
        return Jwt.GenerateToken(user);
    }
}
