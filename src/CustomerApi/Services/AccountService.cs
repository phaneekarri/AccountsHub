using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerApi.Dto;
using CustomerEntities;
using CustomerEntities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CustomerApi;

public class AccountService(ILogger<AccountService> logger, IMapper mapper, CustomerDbContext context)  : IAccountService
{
    private readonly ILogger<AccountService> _logger = logger;
    private readonly CustomerDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<int> Create(CreateAccount dto)
    {
        Account account = _mapper.Map<CreateAccount, Account>(dto) ;
        if(dto.owner != null) await  AddOwners(account, [dto.owner]);        
       _context.Accounts.Add(account);
       await _context.SaveChangesAsync();
       return account.Id;
    }

    public async Task<bool> Delete(int Id)
    {
        var accounts = _context.Accounts.Where(c=> c.Id == Id);

        if(accounts == null || accounts.Count()==0)
            throw new KeyNotFoundException("Account not found");
        _context.AccountOwners.RemoveRange(accounts.SelectMany(x=>x.AccountOwners));
        _context.Accounts.RemoveRange(accounts);
        return await _context.SaveChangesAsync() > 0;   
    }

    public async Task<IEnumerable<GetAccount>> GetAll()
    {
        return _mapper.Map<IEnumerable<Account> , IEnumerable<GetAccount>>(await _context.Accounts.Include( a => a.AccountOwners).ToListAsync());
    }

    public async Task<GetAccount> GetBy(int Id)
    {
        return _mapper.Map<Account, GetAccount>(await _context.Accounts.Include( a => a.AccountOwners).FirstOrDefaultAsync(c => c.Id == Id));
    }

    public async Task<bool> Update(int Id, UpdateAccount dto)
    {
        Account account = await _context.Accounts.FindAsync(Id);
        if(account == null ) 
            throw new KeyNotFoundException("Account not found"); 
        _mapper.Map(dto, account);
        _context.Accounts.Update(account);       
        
        return await _context.SaveChangesAsync() > 0 ; 
    }

    private async Task<bool> AddOwners(Account account, IEnumerable<CreateAccountOwner> Owners){
        if(Owners == null && Owners.Count()>0) throw new ArgumentNullException("Null value for owners");
        if(!Owners.GroupBy(item => item.Id).All(g => g.Count() == 1)){
            throw new DuplicateNameException("Duplicate owners are not allowed");
        }
        var clients =  await _context.Clients.Where(entity => Owners.Select(o => o.Id).Contains(entity.Id)).ToListAsync();
        if(Owners.Count() != clients.Count)
        throw new KeyNotFoundException("One or more owners doesn't exist");
        account.AddOwners(clients);
        await _context.SaveChangesAsync();
        return true;
    }

    public async  Task<bool> AddOwners(int Id, IEnumerable<CreateAccountOwner> Owners )
    { 
        var account = await _context.Accounts.Include( a => a.AccountOwners).FirstOrDefaultAsync(c => c.Id == Id);
        return await AddOwners(account, Owners);
    }

    public async Task<bool> DeleteOwner(int Id, int ClientId)
    {  
        var account = await _context.Accounts.Include( a => a.AccountOwners).FirstOrDefaultAsync(c => c.Id == Id);
        account.DeleteOwner(ClientId);
        return await _context.SaveChangesAsync() >0;
    }

    public async Task<bool> DeleteOwners(int Id)
    {
        var account = await _context.Accounts.Include( a => a.AccountOwners).FirstOrDefaultAsync(c => c.Id == Id);
        account.DeleteAllOwners();
        return await _context.SaveChangesAsync() >0;
    }    
}
