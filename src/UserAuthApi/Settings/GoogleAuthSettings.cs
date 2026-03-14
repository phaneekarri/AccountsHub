namespace UserAuthApi.Settings;
internal record AuthSettings
{
   public required string ClientId { get; init;}
   public required string Secret {get; init;}
   public required string RedirectURI {get; init;}

   public required string TokenEndpointURL {get; init;}
   public required string UserInfoEndpointURL {get; init;}
}

internal record GoogleAuthSettings : AuthSettings {};