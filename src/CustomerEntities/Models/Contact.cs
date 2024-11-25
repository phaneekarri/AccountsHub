using InfraEntities;
using InfraEntities.Interfaces;
using CustomerEntities.Interfaces;

namespace CustomerEntities.Models.Contacts
{
    public abstract class Contact<T> : AuditEntity,
    IContact<T>     
    {

        public int Id { get; init; }
        public T Value { get; private set; }
        public int PriorityOrder {get; set;} = 1;

        public virtual void Update(T value)
        {
            this.Value = value;
        }
    }

   
}
