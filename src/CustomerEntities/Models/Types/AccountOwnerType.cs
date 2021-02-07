using System;

namespace CustomerEntities.Models.Types
{
    public abstract class AccountOwnerType : ModelType { }
    public abstract class AccountOwnerType<T> : AccountOwnerType
     where T : ModelType
    {
        protected AccountOwnerType() => SetType<T>();
    }
    public class PrimaryAccountOwner : AccountOwnerType<Primary> { }
    public class SecondaryAccountOwner : AccountOwnerType<Secondary> { }
}
