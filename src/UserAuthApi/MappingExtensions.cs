using UserAuthApi.Dto;
using UserAuthEntities;
using UserAuthEntities.InternalUsers;

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
            Phone = dto.Phone
        };
    }

    // InternalUser mappings
    public static InternalUserDto ToInternalUserDto(this InternalUser internalUser)
    {
        return new InternalUserDto
        {
            Id = internalUser.User.Id,
            Email = internalUser.User.Email,
            Phone = internalUser.User.Phone,
            UserName = internalUser.User.UserName,
            IsMFAEnabled = internalUser.IsMFAEnabled
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