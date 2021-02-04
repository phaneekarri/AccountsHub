using System.Collections.Generic;

namespace LoanEntities.Models
{
    public class AccountOwner
    {
        public int Id { get; set; }
        public Client Client { get; set; }
        public Account Account { get; set; }
    }
}
