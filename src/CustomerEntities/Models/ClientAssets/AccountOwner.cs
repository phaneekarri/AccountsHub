using CustomerEntities.Interfaces;
using InfraEntities;
using InfraEntities.Interfaces;

namespace CustomerEntities.Models
{
    
    public class AccountOwner :
        AuditableSoftDeleteEntity, 
        IAuditEntity, 
        ISoftDelete,
        IHasClientRelation,
        IHasAccountRelation,
        IHasPriorityOrder
    {    
        public int Id { get; set; }       
        public int ClientId {get; private set;} 
        public Client Client { get; private set; }
        public int AccountId {get; private set;}
        public Account Account { get; private set; }
        public bool IsActive { get; set; }
        public int PriorityOrder {get; set;}

        public void Update(Account account)
        {
            Account = account;
            AccountId = account.Id;            
        }
        public void Update(Client client)
        {
            Client = client;
            ClientId = client.Id;
            
        }
        
    }

}
