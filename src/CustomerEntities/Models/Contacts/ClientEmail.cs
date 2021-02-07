using CustomerEntities.Models.Types;
using System;

namespace CustomerEntities.Models.Contacts
{
    public class ClientEmail<TContactType> : ClientEmailContact
        where TContactType : ContactType
    {
        public ClientEmail() 
            => ContactType = Activator.CreateInstance<TContactType>();
    }

    public class ClientEmailContact : ClientContact<string>
    {
    }
}
