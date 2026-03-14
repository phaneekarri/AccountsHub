using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Options;
using UserAuthApi.Exceptions;
using UserAuthApi.Settings;

namespace UserAuthApi.AuthProviders;

internal interface IAuthToken{
     public string AccessToken {get; init;}
}
internal record AuthProviderUser(string Id, string Email, string Name);
internal record GoogleUser(string Id, string Email, string Name) 
: AuthProviderUser(Id, Email, Name);
internal abstract class AuthProvider<TToken, TUser>
(
    IOptions<AuthSettings> settings, 
    IHttpClientFactory httpClientFactory,
    ILogger logger
)
where TToken : IAuthToken
where TUser :  AuthProviderUser
{
    protected readonly string ClientId = settings.Value.ClientId;
    protected readonly string ClientSecret = settings.Value.Secret;
    protected readonly string RedirectUri = settings.Value.RedirectURI;
    protected readonly  string UserInfoURL = settings.Value.UserInfoEndpointURL;
    protected readonly string TokenEndpointURL = settings.Value.TokenEndpointURL;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    protected async Task<TToken> FetchTokenAsync(
        IDictionary<string, string> tokenParams,
        CancellationToken cancellation = default)
    {
        using var client = httpClientFactory.CreateClient();
        using var tokenResponse =
            await client.PostAsync(TokenEndpointURL,
            new FormUrlEncodedContent(tokenParams),
            cancellation);
        tokenResponse.EnsureSuccessStatusCode();
        return await tokenResponse.Content.ReadFromJsonAsync<TToken>(
            JsonOptions,
            cancellationToken: cancellation)
            ??throw new AuthorizationException("Invalid Authorization Token fetched.");
    }

    protected virtual async Task<TToken> GetTokenAsync(
        string code, CancellationToken cancellation = default)
    {
        try{
            if(string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Authorization code is missing!");
            return await FetchTokenAsync(
                new Dictionary<string, string>
                {
                    { "code", code! },
                    { "client_id", ClientId },
                    { "client_secret", ClientSecret},
                    { "redirect_uri", RedirectUri },
                    { "grant_type", "authorization_code" }
                }, cancellation);
        }
       catch(Exception ex)
       {
         logger.LogError(ex, "Authorized Token retrieval Failed.");
         throw new AuthorizationException("Authorized Token retrieval Failed.", ex);
       } 
    }

    internal async Task<TUser> AuthenticateAsync(
        string code , CancellationToken cancellation = default)
    {
        var token = await GetTokenAsync(code, cancellation);
        if(token?.AccessToken is null){
            logger.LogError("Authorized Token retrieval Failed.");
            throw new AuthorizationException($"Authorized Token retrieval failed");
        }
        return await GetUserInfoAsync(token.AccessToken, cancellation);       
    }

    protected virtual async Task<TUser> GetUserInfoAsync(
        string accessToken, CancellationToken cancellation = default)
    {
         using var client = httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization 
            = new AuthenticationHeaderValue("Bearer", accessToken);
        using var response = await client.GetAsync(UserInfoURL);
        response.EnsureSuccessStatusCode(); 
        var user = await response.Content.ReadFromJsonAsync<TUser>(
                    JsonOptions,cancellationToken: cancellation);
        if(user is null){
            logger.LogError("Failed to retireve User Info from Auth Provider.");
            throw new Exception("Failed to retireve User Info from Auth Provider.");
        }
        return user;
    }
}