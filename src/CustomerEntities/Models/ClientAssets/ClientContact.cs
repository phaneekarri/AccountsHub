using CustomerEntities.Interfaces;
using CustomerEntities.Models.Contacts;
using InfraEntities.Interfaces;

namespace CustomerEntities.Models;
public class ClientEmailContact : ClientContact<string> {}
public class ClientPhoneContact : ClientContact<string> {}
public class ClientAddressContact : ClientContact<Address>
{
    public int AddressId { get; set; }
    public override void Update(Address value)
    {
        base.Update(value);
        this.AddressId = value.Id;
    }    
}
public class ClientContact<T> : Contact<T>,
    IHasClientRelation,
    IHasPriorityOrder
{        
    public Client Client 
    { 
        get ;
        private set ;        
    }
    public int ClientId 
    { 
        get ;
        private set ;       
    } 

    public bool IsPrimary => this.PriorityOrder == 1    ;

    public void Update(Client client)
    {
        Client = client;
        ClientId = client.Id;
    }
    
}


