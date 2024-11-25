using System.Collections.Generic;
using InfraEntities;
using InfraEntities.Interfaces;

namespace CustomerEntities.Models
{
    public class Account : AuditableSoftDeleteEntity, 
    IAuditEntity, 
    ISoftDelete   
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<AccountOwner> AccountOwners { get; set; } = new HashSet<AccountOwner>();
    }
}
