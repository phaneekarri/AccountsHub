using System.Linq;
using AutoMapper;
using CustomerApi.Dto;
using CustomerEntities;
using CustomerEntities.Models;

public class AccountOwnerClientMappingResolver : IValueResolver<CreateAccountOwner, AccountOwner, Client>
{
    private readonly CustomerDbContext _dbContext;

    public AccountOwnerClientMappingResolver(CustomerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Client Resolve(CreateAccountOwner source, AccountOwner destination, Client destMember, ResolutionContext context)
    {
        return _dbContext.Clients.FirstOrDefault(x => x.Id == source.ClientId);
    }
}