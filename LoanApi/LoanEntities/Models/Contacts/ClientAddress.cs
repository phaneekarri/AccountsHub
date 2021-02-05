using LoanEntities.Models.Types;
using System;

namespace LoanEntities.Models.Contacts
{
    public class ClientAddress<TContactType> : ClientAddressContact 
        where TContactType : ContactType
    {
        public ClientAddress() 
            => ContactType = Activator.CreateInstance<TContactType>();
    }
    public class ClientAddressContact : ClientContact<Address>
    {
    }
}
