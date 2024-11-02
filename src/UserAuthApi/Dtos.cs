using System.Data;
using FluentValidation;

namespace UserAuthApi;

public record UserLoginModel(UserIdentifierType UserIdentifierType, string? UserIdentifier);

public class UserLoginValidator : AbstractValidator<UserLoginModel>
{
     public UserLoginValidator()
    {
        RuleFor(x => x.UserIdentifierType)
            .IsInEnum().WithMessage("UserIdentifierType must be 1 or 2.");
      
        When(x => x.UserIdentifierType == UserIdentifierType.Email, () =>
        {
            RuleFor(x => x.UserIdentifier)
                .NotEmpty().WithMessage("userIdentifier is required.")
                .EmailAddress().WithMessage("userIdentifier must be a valid email address.");
        });

        When(x => x.UserIdentifierType == UserIdentifierType.Phone, () =>
        {
            RuleFor(x => x.UserIdentifier)
                .NotEmpty().WithMessage("userIdentifier is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("userIdentifier must be a valid phone number.");
        });
    }

}

public class OtpModel 
{
    public Guid UserId {get; set;}
    public UserIdentifierType IdentifierType {get; set;}
    public string? Otp {get; set;} 
    public int ExpiresInSecs {get; set;}
}
public record OtpVerficationModel(string UserId, string? Otp, UserIdentifierType UserIdentifierType);

public class OtpVerifictionValidator : AbstractValidator<OtpVerficationModel>
{
     public OtpVerifictionValidator()
    {
        RuleFor(x => x.UserIdentifierType)
            .IsInEnum().WithMessage("UserIdentifierType must be 1 or 2.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId cannot be empty")
            .Must((userId) => Guid.TryParse(userId, out _)).WithMessage("UserId must be a valid GUID");

        RuleFor(x=>Convert.ToInt32(x.Otp)            )
        .GreaterThan(0).WithMessage("Otp is required")
        .InclusiveBetween(10000, 99999).WithMessage("Invalid Otp");
    }
}
