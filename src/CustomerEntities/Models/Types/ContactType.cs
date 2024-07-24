using InfraEntities.ModelType;

namespace CustomerEntities.Models.Types
{
    public abstract class ContactType : ModelType { }
    public abstract class ContactType<T> : ContactType
     where T : ModelType
    {
        public ContactType() => SetType<T>();
    }
    public class PrimaryContact : ContactType<Primary> { }
    public class SecondaryContact : ContactType<Secondary> { }
}
