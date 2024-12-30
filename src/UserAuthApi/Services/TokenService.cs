using AutoMapper;
using UserAuthApi.Dto;
using UserAuthEntities;

namespace UserAuthApi.Services;

public class TokenService(JwtService jwt, ILogger<TokenService> logger, AuthDBContext context, IMapper mapper)
: BaseService<TokenService, AuthDBContext>(logger, mapper, context ), ITokenService
{
  private readonly JwtService Jwt =jwt;

    public AuthTokenModel GenerateToken(User user)
    {
        var token =  Jwt.GenerateToken(user);
        Context.UserAccessTokens.Add(new UserAccessToken
        {
           Token = token.accessToken,
           ExpiryIn = token.expiresInSecs,
           UserId = user.Id,
        });
        return token;
    }

    public static AuthTokenModel CreateAuthToken(string accessToken) => new AuthTokenModel(accessToken, 3600);
}
