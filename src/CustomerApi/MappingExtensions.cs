using System.Linq;
using CustomerApi.Dto;
using CustomerEntities.Models;

namespace CustomerApi;

public static class MappingExtensions
{
    // Client mappings
    public static GetClient ToGetClient(this Client client)
    {
        return new GetClient
        {
            Id = client.Id,
            FirstName = client.FirstName,
            LastName = client.LastName,
            DOB = client.DOB,
            Age = client.Age
        };
    }

    public static Client ToClient(this CreateClient dto)
    {
        return new Client
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DOB = dto.DOB
        };
    }

    public static void UpdateFrom(this Client client, UpdateClient dto)
    {
        client.FirstName = dto.FirstName;
        client.LastName = dto.LastName;
        client.DOB = dto.DOB;
    }

    // Account mappings
    public static GetAccount ToGetAccount(this Account account)
    {
        return new GetAccount
        {
            Id = account.Id,
            Title = account.Title,
            Owners = account.AccountOwners.Select(ao => ao.ToGetAccountOwner())
        };
    }

    public static Account ToAccount(this CreateAccount dto)
    {
        return new Account
        {
            Title = dto.Title
        };
    }

    public static void UpdateFrom(this Account account, UpdateAccount dto)
    {
        account.Title = dto.Title;
    }

    // AccountOwner mappings
    public static GetAccountOwner ToGetAccountOwner(this AccountOwner accountOwner)
    {
        return new GetAccountOwner
        {
            Id = accountOwner.Id,
            ClientId = accountOwner.ClientId
        };
    }
}