using CustomerEntities.Models.Types;
using System;


namespace CustomerEntities.Models
{
    public class AccountOwner : AuditEntity, ISoftDelete
    {
        public int Id { get; set; }
        public Client Client { get; set; }
        public Account Account { get; set; }
        public bool IsActive { get; set; }
         public bool IsDeleted { get; set; }
        public AccountOwnerType  AccountOwnerType {get; set;}
    }
}
