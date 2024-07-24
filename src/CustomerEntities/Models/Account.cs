using System.Collections.Generic;
using InfraEntities;

namespace CustomerEntities.Models
{
    public class Account : AuditEntity, ISoftDelete    
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<AccountOwner> AccountOwners { get; set; } = new HashSet<AccountOwner>();
        public bool IsDeleted { get; set; }
    }
}
