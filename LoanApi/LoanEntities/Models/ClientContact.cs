using LoanEntities.Models.Contacts;

namespace LoanEntities.Models
{
    public class ClientContact<T> : Contact<T>
    {
        public Client Client { get; set; }
    }
}
