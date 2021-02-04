using System;
using System.Collections.Generic;
using System.Text;

namespace LoanEntities.Models.Contacts
{
    public class ClientEmail<TContactType> : ClientEmailContact where TContactType : ContactType
    {
        public ClientEmail()
        {
            ContactType = Activator.CreateInstance<TContactType>();
        }
    }
}
