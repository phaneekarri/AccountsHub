using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CustomerApi.Dto;
using CustomerEntities;
using CustomerEntities.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerApi;

public class AccountService(CustomerDbContext context)  
 : IAccountService
{
    public async Task<int> Create(CreateAccount dto)
    {
        Account account = dto.ToAccount();
        context.Accounts.Add(account);
        if(dto.owner != null) await AddOwners(account, [dto.owner]);        
        await context.SaveChangesAsync();
        return account.Id;
    }

    public async Task<bool> Delete(int Id)
    {
        var accounts = context.Accounts.Where(c=> c.Id == Id);

        if(accounts == null || accounts.Count()==0)
            throw new KeyNotFoundException("Account not found");
        context.AccountOwners.RemoveRange(accounts.SelectMany(x=>x.AccountOwners));
        context.Accounts.RemoveRange(accounts);
        return await context.SaveChangesAsync() > 0;   
    }

    public async Task<IEnumerable<GetAccount>> GetAll()
    {
        return (await context.Accounts.Include( a => a.AccountOwners).ToListAsync())
            .Select(a => a.ToGetAccount());
    }

    public async Task<GetAccount> GetBy(int Id)
    {
        var account = await context.Accounts
            .Include( a => a.AccountOwners)
            .FirstOrDefaultAsync(c => c.Id == Id);
        return account?.ToGetAccount();
    }

    public async Task<bool> Update(int Id, UpdateAccount dto)
    {
        Account account = await context.Accounts.FindAsync(Id);
        if(account == null ) 
            throw new KeyNotFoundException("Account not found"); 
        account.UpdateFrom(dto);
        context.Accounts.Update(account);       
        
        return await context.SaveChangesAsync() > 0 ; 
    }

    private async Task AddOwners(Account account, IEnumerable<CreateAccountOwner> Owners){
        if(Owners == null && Owners.Count()>0) throw new ArgumentNullException("Null value for owners");
        if(!Owners.GroupBy(item => item.Id).All(g => g.Count() == 1)){
            throw new DuplicateNameException("Duplicate owners are not allowed");
        }
        var clients =  await context.Clients
                                .Where(entity =>
                                         Owners.Select(o => o.Id).Contains(entity.Id))
                                .ToListAsync();
        if(Owners.Count() != clients.Count)
        throw new KeyNotFoundException("One or more owners doesn't exist");
        account.AddOwners(clients);
    }

    public async  Task<bool> AddOwners(int Id, IEnumerable<CreateAccountOwner> Owners )
    { 
        var account = await context.Accounts
        .Include( a => a.AccountOwners)
        .FirstOrDefaultAsync(c => c.Id == Id);
        await AddOwners(account, Owners);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteOwner(int Id, int ClientId)
    {  
        var account = await context.Accounts
        .Include( a => a.AccountOwners)
        .FirstOrDefaultAsync(c => c.Id == Id);
        if (account == null) throw new KeyNotFoundException("Account not found");
        account.DeleteOwner(ClientId);
        return await context.SaveChangesAsync() >0;
    }

    public async Task<bool> DeleteOwners(int Id)
    {
        var account = await context.Accounts
        .Include( a => a.AccountOwners)
        .FirstOrDefaultAsync(c => c.Id == Id);
        if (account == null) throw new KeyNotFoundException("Account not found");
        account.DeleteAllOwners();
        return await context.SaveChangesAsync() >0;
    }    
}
