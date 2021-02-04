using System.Collections.Generic;

namespace LoanEntities.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<AccountOwner> AccountOwners { get; set; } = new HashSet<AccountOwner>();
    }
}
