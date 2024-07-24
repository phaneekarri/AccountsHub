using CustomerEntities.Models.Types;
using InfraEntities;

namespace CustomerEntities.Models.Contacts
{
    public abstract class Contact<T> : AuditEntity
    {
        public ContactType ContactType { get; set; }
        public int Id { get; set; }
        public T Value { get; set; }
    }
}
