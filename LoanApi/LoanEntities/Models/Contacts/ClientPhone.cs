using LoanEntities.Models.Types;
using System;

namespace LoanEntities.Models.Contacts
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
