using CustomerEntities.Models.Contacts;

namespace CustomerEntities.Models
{
    public class ClientContact<T> : Contact<T>
    {
        public Client Client { get; set; }
    }
}
