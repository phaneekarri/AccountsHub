using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerApi.Dto;
using CustomerEntities;
using CustomerEntities.Models;
using Microsoft.Extensions.Logging;

namespace CustomerApi;

public class AccountOwnerService : IAccountOwnerService
{
    private readonly ILogger<AccountOwnerService> _logger;
    private readonly CustomerDbContext _context;
    private readonly IMapper _mapper;

    public AccountOwnerService(ILogger<AccountOwnerService> logger, IMapper mapper, CustomerDbContext context) 
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }  

    
  public async  Task<bool> AddOwnersToAccount(int Id, IEnumerable<CreateAccountOwner> Owners )
  { 
      if(Owners == null && Owners.Count()>0) throw new ArgumentNullException("Null value for owners");
      bool isAnyCreated =false;
        IEnumerable<AccountOwner> existingAccountOwners = _context.AccountOwners.Where(x=> x.AccountId == Id);
        IEnumerable<AccountOwner> ownerstoCreate = _mapper.Map<IEnumerable<AccountOwner>>(Owners);
        ownerstoCreate = ownerstoCreate.Except(existingAccountOwners, new AccountOwnersComparer());
        var accountToCreateOwners = _context.Accounts.SingleOrDefault(a => a.Id == Id);
        if(accountToCreateOwners == null) throw new KeyNotFoundException("Account doesn't exist");
      if(ownerstoCreate.Any())
      {
         await _context.AccountOwners.AddRangeAsync(ownerstoCreate.Select( o => {o.Update(accountToCreateOwners); return o;}));
         isAnyCreated = await _context.SaveChangesAsync() > 0;
      }
      return isAnyCreated;
  }

  public async Task<bool> DeleteOwnerToAccount(int AccountId, int ClientId)
  {
    IQueryable<AccountOwner> entities = _context.AccountOwners.Where(x => x.AccountId == AccountId && x.ClientId == ClientId);
    if(entities.Any()) _context.AccountOwners.RemoveRange(entities);
    else  throw new KeyNotFoundException("No matching item found");    
    return await _context.SaveChangesAsync() >0;
  }

  public async Task<bool> DeleteOwnersToAccount(int AccountId)
  {
    IQueryable<AccountOwner> entities = _context.AccountOwners.Where(x => x.AccountId == AccountId);
    if(entities.Any()) _context.AccountOwners.RemoveRange(entities);
    else  throw new KeyNotFoundException("No matching item found");    
    return await _context.SaveChangesAsync() >0;
  }
}

public class AccountOwnersComparer : IEqualityComparer<AccountOwner>
{
    public bool Equals(AccountOwner x, AccountOwner y)
    {
        return x.ClientId == y.ClientId && x.AccountId == y.AccountId && x.PriorityOrder == y.PriorityOrder ;
    }

    public int GetHashCode([DisallowNull] AccountOwner obj)
    {
         if (obj == null) return 0;
        int hashClientId = obj.ClientId.GetHashCode();
        int hashAccountTypeId = obj.AccountId.GetHashCode();
        int hashPriorityOrder = obj.PriorityOrder.GetHashCode();
        
        return hashClientId ^ hashAccountTypeId ^ hashPriorityOrder;
    }
}