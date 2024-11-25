
using CustomerEntities.Models;

namespace CustomerEntities.Interfaces;
public interface IHasClientRelation : IUpdateEntity<Client>
{
    int ClientId {get;} 
    Client Client { get;}

}

public interface IHasAccountRelation : IUpdateEntity<Account>
{
    int AccountId {get;} 
    Account Account { get; }
}
