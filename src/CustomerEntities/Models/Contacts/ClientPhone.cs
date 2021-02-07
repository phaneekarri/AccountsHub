using CustomerEntities.Models.Types;
using System;

namespace CustomerEntities.Models.Contacts
{
    public class ClientPhone<TContactType> : ClientPhoneContact 
        where TContactType : ContactType
    {
        public ClientPhone() 
            => ContactType = Activator.CreateInstance<TContactType>();
    }
    public class ClientPhoneContact : ClientContact<string>
    {
    }
}
