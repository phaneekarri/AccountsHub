
using UserAuthEntities;

namespace UserAuthApi.Dto;

public class UserDto {
    public Guid Id {get; set; } 
    public string? Email {get; set; } 
    public string? Phone {get; set; } 
    public string? UserName {get; set; } 
}
public class InternalUserDto : UserDto
{
    public bool IsMFAEnabled {get; set; }
}
public record UserLoginModel(UserIdentifierType UserIdentifierType, string? UserIdentifier);
public record InternalUserLoginModel(string UserName, string PasswordText);
public record InternalUserRegisterModel(string UserName, string PasswordText, string Email, string? Phone);
public record SendOtpModel(string UserId, UserIdentifierType Receiver)
{
    public Guid Id => 
    Guid.TryParse(UserId, out Guid guid)? guid : default;
}
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