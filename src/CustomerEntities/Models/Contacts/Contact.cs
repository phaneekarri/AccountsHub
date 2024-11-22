using CustomerEntities.Models.Types;
using InfraEntities;

namespace CustomerEntities.Models.Contacts
{
    public abstract class Contact<T> : AuditEntity
    {
        private ContactType _contactType;
        public ContactType ContactType 
        { 
            get => _contactType;
            set
            {
                _contactType =value;
                _contactTypeId = value.Id;
            } 
        }

        private int _contactTypeId;
        public int ContactTypeId 
        { 
            get => _contactTypeId;
            set
            {
                _contactTypeId = value;
                if (_contactType == null || _contactType.Id != value)
                {
                    _contactType = new ContactType{Id = value};
                }                
            }
        }
        public int Id { get; set; }
        public T Value { get; set; }
    }
}
