using System.Collections.Generic;

namespace CustomerEntities.Models
{
    public class Account : AuditEntity, ISoftDelete    
    {
        public int Id { get; set; }
        public string Title { get; set; }
         public bool IsDeleted { get ; set ; } = false;
        public ICollection<AccountOwner> AccountOwners { get; set; } = new HashSet<AccountOwner>();
    }
}
