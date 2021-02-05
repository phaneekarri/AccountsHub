using LoanEntities.Models.Types;

namespace LoanEntities.Models.Contacts
{
    public abstract class Contact<T>
    {
        public ContactType ContactType { get; set; }
        public int Id { get; set; }
        public T Value { get; set; }
    }
}
