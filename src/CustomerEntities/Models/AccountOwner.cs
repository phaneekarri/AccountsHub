using CustomerEntities.Models.Types;
using InfraEntities;
using InfraEntities.Interfaces;


namespace CustomerEntities.Models
{
    public class AccountOwner : AuditableSoftDeleteEntity, IAuditEntity, ISoftDelete
    {
        public int Id { get; set; }       
        public int ClientId {get; set;} 
        public Client Client { get; set; }
        public int AccountId {get; set;}
        public Account Account { get; set; }
        public bool IsActive { get; set; }
        public int AccountOwnerTypeId {get; set;}
        public AccountOwnerType  AccountOwnerType {get; set;}
    }
}
