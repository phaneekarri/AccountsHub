using LoanEntities.Models.Contacts;
using System.Collections.Generic;
using System.Linq;

namespace LoanEntities.Models
{
    public class Client 
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ClientEmailContact PrimaryEmail
        {
            get => EmailContacts.SingleOrDefault(x => x.ContactType.Description == "Primary");
            private set { }
        }

        public ClientAddressContact PrimaryAddress
        {
            get => AddressContacts.SingleOrDefault(x => x.ContactType.Description == "Primary");
            private set { }
        }
        public ClientPhoneContact PrimaryPhone
        {
            get => PhoneContacts.SingleOrDefault(x => x.ContactType.Description == "Primary");
            private set { }
        }

        public ICollection<ClientEmailContact> SecondaryEmail
        {
            get => EmailContacts.Where(x => x.ContactType.Description == "Secondary").ToHashSet();
            private set { }
        }

        public ICollection<ClientAddressContact> SecondaryAddresses
        {
            get => AddressContacts.Where(x => x.ContactType.Description == "Secondary").ToHashSet();
            private set { }
        }

        public ICollection<ClientPhoneContact> SecondaryPhone
        {
            get => PhoneContacts.Where(x => x.ContactType.Description == "Secondary").ToHashSet();
            private set { }
        }

        public ICollection<Account> Accounts { get; set; }= new HashSet<Account>();
        public ICollection<ClientEmailContact> EmailContacts { get; set; } = new HashSet<ClientEmailContact>();
        public ICollection<ClientAddressContact> AddressContacts { get; set; } = new HashSet<ClientAddressContact>();
        public ICollection<ClientPhoneContact> PhoneContacts { get; set; } = new HashSet<ClientPhoneContact>();

        private void AddPrimaryContact<T>(ICollection<T> contacts, T primaryContact ) 
            //where T : Contact
        {
          // contacts.Where(x => x.)Contains() 
        }

    }
}
