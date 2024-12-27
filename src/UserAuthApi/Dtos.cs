
namespace UserAuthApi.Dto;

public record UserLoginModel(UserIdentifierType UserIdentifierType, string? UserIdentifier);

public class OtpModel 
{
    public Guid UserId {get; set;}
    public UserIdentifierType IdentifierType {get; set;}
    public string? Otp {get; set;} 
    public int ExpiresInSecs {get; set;}
}
public record OtpVerficationModel(string UserId, string? Otp, UserIdentifierType UserIdentifierType)
{
    public Guid Id => 
        Guid.TryParse(UserId, out Guid guid)? guid : default;

}

public record AuthTokenModel(string accessToken , int expiresInSecs);