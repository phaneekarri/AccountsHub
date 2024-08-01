using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerApi.Dto;
using CustomerEntities;
using CustomerEntities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CustomerApi;

public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly CustomerDbContext _context;
    private readonly IMapper _mapper;

    public AccountService(ILogger<AccountService> logger, IMapper mapper, CustomerDbContext context) 
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }    
    public async Task<int> Create(CreateAccount dto)
    {
        Account account = _mapper.Map<CreateAccount, Account>(dto) ;
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
            throw new KeyNotFoundException("Account not found}"); 
        _mapper.Map(dto, account);
        _context.Accounts.Update(account);       
        
        return await _context.SaveChangesAsync() > 0 ; 
    }
}
