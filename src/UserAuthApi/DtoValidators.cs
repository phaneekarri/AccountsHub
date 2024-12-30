using System.Data;
using FluentValidation;
using UserAuthEntities;

namespace UserAuthApi.Dto.Validators;
public class OtpVerifictionValidator : AbstractValidator<OtpVerficationModel>
{
     public OtpVerifictionValidator()
    {
        RuleFor(x => x.UserIdentifierType)
        .IsInEnum().WithMessage("UserIdentifierType must be 1 or 2.");

        RuleFor(x => x.UserId)
        .NotEmpty().WithMessage("UserId cannot be empty");

        RuleFor(x => x.Id)            
        .Must((Id) => Id != default).WithMessage("UserId must be a valid GUID");

        RuleFor(x=>Convert.ToInt32(x.Otp)            )
        .GreaterThan(0).WithMessage("Otp is required")
        .InclusiveBetween(10000, 99999).WithMessage("Invalid Otp");
    }
}

public class UserLoginValidator : AbstractValidator<UserLoginModel>
{
     public UserLoginValidator()
    {
        RuleFor(x => x.UserIdentifierType)
            .IsInEnum().WithMessage("Invalid Identifier type");
      
        When(x => x.UserIdentifierType == UserIdentifierType.Email, () =>
        {
            RuleFor(x => x.UserIdentifier)
                .NotEmpty().WithMessage("userIdentifier is required.")
                .EmailAddress().WithMessage("Must be a valid email address.");
        });

        When(x => x.UserIdentifierType == UserIdentifierType.Phone, () =>
        {
            RuleFor(x => x.UserIdentifier)
                .NotEmpty().WithMessage("userIdentifier is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Must be a valid phone number.");
        });
    }

}

public class InternalUserLoginValidator : AbstractValidator<InternalUserLoginModel>
{
    public InternalUserLoginValidator()
    {
        RuleFor(x => x.UserName)
        .NotEmpty().WithMessage("Username is required")
        .MinimumLength(5).WithMessage("Username must be at least 5 characters.")
        .MaximumLength(50).WithMessage("Username must be at most 50 characters");

        RuleFor(x => x.PasswordText)        
        .SetValidator(new PasswordValidator());
    }
}

public class InternalUserRegisterValidator : AbstractValidator<InternalUserRegisterModel>
{
    public InternalUserRegisterValidator()
    {
        RuleFor(x => x.UserName)
        .NotEmpty().WithMessage("Username is required")
        .MinimumLength(5).WithMessage("Username must be at least 5 characters.")
        .MaximumLength(50).WithMessage("Username must be at most 50 characters");

        RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email is required")
        .EmailAddress();

        RuleFor(x => x.PasswordText)        
        .SetValidator(new PasswordValidator());
    }
}
public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(password => password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.")
            .Must(password => !password.Contains(" ")).WithMessage("Password must not contain spaces.");
    }
}

public class SendOtpValidator : AbstractValidator<SendOtpModel>
{
    public SendOtpValidator()
    {
        RuleFor(x=> x.UserId)
        .Must(x => Guid.TryParse(x, out _)).WithMessage("User is invalid");

        RuleFor(x=> x.Receiver)
        .IsInEnum().WithMessage("Receiver is invalid.");
    }
}