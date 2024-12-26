using System;
using System.Collections.Generic;
using System.Linq;
using CustomerEntities;
using FluentValidation;

namespace CustomerApi.Dto.Validators;

public class CreateClientValidator : AbstractValidator<CreateClient>{
    public CreateClientValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required.");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required.");
        RuleFor(x=> x.DOB)
             .GreaterThan(new DateOnly(1900,1,1))
             .WithMessage("DOB cannot be less than 01/01/1900");
        RuleFor(x=> x.DOB)
             .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
             .WithMessage("Future data is not valid for DOB");                        
    }
}

public class UpdateClientValidator : AbstractValidator<UpdateClient>{
    public UpdateClientValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required.");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required.");
        RuleFor(x=> x.DOB)
             .GreaterThan(new DateOnly(1900,1,1))
             .WithMessage("DOB cannot be less than 01/01/1900");
        RuleFor(x=> x.DOB)
             .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
             .WithMessage("Future data is not valid for DOB");                        
    }
}

public class CreateAccountOwnerValidator : AbstractValidator<CreateAccountOwner>
{
    private readonly CustomerDbContext _db;
    public CreateAccountOwnerValidator(CustomerDbContext db)
    {
        _db  = db;
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid owner.");
        RuleFor(x => x.Id)
            .Must(ClientExistInDatabase)
            .WithMessage("Invalid owner.");                      
    }

    private bool ClientExistInDatabase(int id)
    {
        return _db.Clients.Any(u => u.Id == id);
    }
}
public class CreateAccountOwnerListValidator : AbstractValidator<IEnumerable<CreateAccountOwner>>
{
        public CreateAccountOwnerListValidator(CreateAccountOwnerValidator itemValidator )
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("Atleast one owner is required");
        RuleForEach(x => x)
            .SetValidator(itemValidator)
            .WithMessage("One or more owner are invalid. Please check the details.");
        RuleFor(x => x)
            .Must(x => x.GroupBy(item => item.Id).All(g => g.Count() == 1))
            .WithMessage("Duplicate owners are not allowed.");
    }

}

public class CreateAccountValidator : AbstractValidator<CreateAccount>
{
    public CreateAccountValidator(IValidator<CreateAccountOwner> ownerValidator)
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.owner)
            .NotNull().WithMessage("Owner is required.");
        RuleFor(x => x.owner)
            .SetValidator(ownerValidator);
    }
}

public class UpdateAccountValidator : AbstractValidator<UpdateAccount>
{
    public UpdateAccountValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.");                      
    }
}
