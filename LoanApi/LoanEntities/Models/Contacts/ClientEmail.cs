using LoanEntities.Models.Types;
using System;

namespace LoanEntities.Models.Contacts
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
