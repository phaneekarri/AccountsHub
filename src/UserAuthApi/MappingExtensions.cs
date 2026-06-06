using UserAuthApi.Dto;
using UserAuthEntities;

namespace UserAuthApi;

public static class MappingExtensions
{
    // User mappings
    public static UserDto ToUserDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Phone = user.Phone,
            UserName = user.UserName
        };
    }

    public static User ToUser(this UserLoginModel dto)
    {
        return new User
        {
            Email = dto.UserIdentifierType == UserIdentifierType.Email ? dto.UserIdentifier : null,
            Phone = dto.UserIdentifierType == UserIdentifierType.Phone ? dto.UserIdentifier : null
        };
    }

    public static User ToUser(this InternalUserRegisterModel dto)
    {
        return new User
        {
            Email = dto.Email,
            Phone = dto.Phone,
            UserName = dto.UserName
        };
    }

    // User mappings to DTO
    public static InternalUserDto ToInternalUserDto(this User user)
    {
        return new InternalUserDto
        {
            Id = user.Id,
            Email = user.Email,
            Phone = user.Phone,
            UserName = user.UserName,
            IsMFAEnabled = user.MfaEnabled
        };
    }

    // UserOtp mappings
    public static OtpModel ToOtpModel(this UserOtp userOtp)
    {
        return new OtpModel
        {
            UserId = userOtp.UserId,
            IdentifierType = userOtp.UserIdentifierType,
            Otp = userOtp.Token,
            ExpiresInSecs = userOtp.ExpiryIn
        };
    }
}