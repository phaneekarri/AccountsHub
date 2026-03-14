using Microsoft.Extensions.Options;
using UserAuthApi.AuthProviders;
using UserAuthApi.Services;
using UserAuthApi.Settings;

internal class GoogleAuthProvider(
    IOptions<GoogleAuthSettings> googleSettings,
    IHttpClientFactory httpClientFactory,
    ILogger<GoogleAuthProvider> logger
) : AuthProvider<TokenResponse, GoogleUser>(googleSettings, httpClientFactory,logger){}