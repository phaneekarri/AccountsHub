using CustomerEntities.Models.Types;
using System;

namespace CustomerEntities.Models.Contacts
{
    public class ClientAddressContact : ClientContact<Address>
    {
        public int AddressId { get; set; }
    }
}
