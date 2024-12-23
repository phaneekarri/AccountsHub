using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CustomerEntities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CustomerApi.Dto;

public class GetClient {
    public int Id {get; set;} 
    public string FirstName{get; set;} 
    public string LastName{get; set;} 
    public DateOnly DOB {get; set;}
    public int Age {get; set;}
}
public class  CreateClient {
    public string FirstName {get; set;} 
    public string LastName {get; set;} 
    public DateOnly DOB {get; set;}  

}

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

public class  UpdateClient {
    public string FirstName {get; set;} 
    public string LastName {get; set;} 
    public DateOnly DOB {get; set;}  

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

public class GetAccountOwner
{
   public int Id {get; set;} 
   public int ClientId {get; set;}
}
public class CreateAccountOwner
{
    public  int ClientId {get; set;} 
}
public class CreateAccountOwnerValidator : AbstractValidator<CreateAccountOwner>
{
    private readonly CustomerDbContext _db;
    public CreateAccountOwnerValidator(CustomerDbContext db)
    {
        _db  = db;
        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("Invalid client.");
        RuleFor(x => x.ClientId)
            .MustAsync(ClientExistInDatabase)
            .WithMessage("Invalid client.");                      
    }

    private async Task<bool> ClientExistInDatabase(int id, CancellationToken cancellationToken)
    {
        return await _db.Clients.AnyAsync(u => u.Id == id, cancellationToken);
    }
}

public class UpdateAccountOwner
{ 
    public int ClientId {get; set;}  
};
public class UpdateAccountOwnerValidator : AbstractValidator<UpdateAccountOwner>
{
    private readonly CustomerDbContext _db;
    public UpdateAccountOwnerValidator(CustomerDbContext db)
    {
        _db  = db;
        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("Invalid client.");
        RuleFor(x => x.ClientId)
            .MustAsync(ClientExistInDatabase)
            .WithMessage("Invalid client.");                      
    }

    private async Task<bool> ClientExistInDatabase(int id, CancellationToken cancellationToken)
    {
        return await _db.Clients.AnyAsync(u => u.Id == id, cancellationToken);
    }
}
public class GetAccount {
    public int Id{get; set;} 
    public string Title {get; set;}  
    public IEnumerable<GetAccountOwner> Owners {get; set;}
}
public class CreateAccount 
{ 
    public string Title {get; set;}      
}
public class CreateAccountValidator : AbstractValidator<CreateAccount>
{
    public CreateAccountValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.");                      
    }
}
public class UpdateAccount 
{
    public string Title {get; set;} 
}

public class UpdateAccountValidator : AbstractValidator<UpdateAccount>
{
    public UpdateAccountValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.");                      
    }
}