using UserAuthApi.Dto;
using UserAuthApi.Exceptions;
using UserAuthApi.Services;
using UserAuthEntities;

namespace UserAuthApi.Process;

public interface IOAuthLogin
{
    Task<AuthTokenModel> AuthenticateWithGoogle(string code, CancellationToken cancellation = default);
}

public class OAuthLogin(
    ILogger<OAuthLogin> logger,
    GoogleAuthProvider authProvider,
    IUserService userService,
    ITokenService tokenService) : IOAuthLogin
{
    private readonly ILogger<OAuthLogin> _logger = logger;
    private readonly GoogleAuthProvider _authProvider = authProvider;
    private readonly IUserService _userService = userService;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<AuthTokenModel> AuthenticateWithGoogle(string code, CancellationToken cancellation = default)
    {
        if (string.IsNullOrEmpty(code))
            throw new ValidationException("Authorization code is missing");

        var googleUser = await _authProvider.AuthenticateAsync(code, cancellation);
        if (googleUser == null || string.IsNullOrEmpty(googleUser.Email))
            throw new ValidationException("Failed to retrieve user information from Google");

        // Create or link OAuth user
        var user = await _userService.CreateOrLinkOAuthUser(
            googleUser.Email,
            OAuthProvider.Google,
            googleUser.Id,
            googleUser.Email);

        if (user == null)
            throw new ValidationException("Failed to create or link user");

        return _tokenService.GenerateToken(user);
    }
}
