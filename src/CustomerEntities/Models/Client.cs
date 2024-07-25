using CustomerEntities.Models.Contacts;
using CustomerEntities.Models.Types;
using InfraEntities;
using InfraEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerEntities.Models
{
    public class Client : AuditableSoftDeleteEntity, IAuditEntity, ISoftDelete
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DOB { get; set; }

        public int Age { get 
            {
                var age = DateTime.Today.Year - DOB.Year;
                if (DateTime.Today.Day > DOB.Day) age++;
                return age;
            }
        }
        public ClientEmailContact PrimaryEmail
        {
            get => EmailContacts.SingleOrDefault(x => x.ContactType.Description == "Primary");
        }
        public ClientAddressContact PrimaryAddress
        {
            get => AddressContacts.SingleOrDefault(x => x.ContactType.Description == "Primary");
        }
        public ClientPhoneContact PrimaryPhone
        {
            get => PhoneContacts.SingleOrDefault(x => x.ContactType.Description == "Primary");
        }

        public ICollection<ClientEmailContact> SecondaryEmail
        {
            get => EmailContacts.Where(x => x.ContactType.Description == "Secondary").ToHashSet();
        }
        public ICollection<ClientAddressContact> SecondaryAddresses
        {
            get => AddressContacts.Where(x => x.ContactType.Description == "Secondary").ToHashSet();
        }
        public ICollection<ClientPhoneContact> SecondaryPhone
        {
            get => PhoneContacts.Where(x => x.ContactType.Description == "Secondary").ToHashSet();
        }

        public ICollection<AccountOwner> Accounts { get; set; }= new HashSet<AccountOwner>();
        public ICollection<ClientEmailContact> EmailContacts { get; set; } = new HashSet<ClientEmailContact>();
        public ICollection<ClientAddressContact> AddressContacts { get; set; } = new HashSet<ClientAddressContact>();
        public ICollection<ClientPhoneContact> PhoneContacts { get; set; } = new HashSet<ClientPhoneContact>();
     
        public void AddPrimaryEmail(string Email ) 
        {
            var existingPrimary = EmailContacts.Single(c => c.ContactType.Description == "Primary");
            existingPrimary.ContactType = new SecondaryContact();
            EmailContacts.Add(new ClientEmail<PrimaryContact>() { Client = this , Value = Email});
        }
        public void AddPrimaryPhone(string Phone)
        {
            var existingPrimary = PhoneContacts.Single(c => c.ContactType.Description == "Primary");
            existingPrimary.ContactType = new SecondaryContact();
            PhoneContacts.Add(new ClientPhone<PrimaryContact>() { Client = this, Value = Phone });
        }
        public void AddPrimaryAddress(Address Address)
        {
            var existingPrimary = AddressContacts.Single(c => c.ContactType.Description == "Primary");
            existingPrimary.ContactType = new SecondaryContact();
            AddressContacts.Add(new ClientAddress<PrimaryContact>() { Client = this, Value = Address });
        }

        public void AddSecondaryEmail(string Email)
        {
            EmailContacts.Add(new ClientEmail<SecondaryContact>() { Client = this, Value = Email });
        }
        public void AddSecondaryPhone(string Phone)
        {
            PhoneContacts.Add(new ClientPhone<SecondaryContact>() { Client = this, Value = Phone });
        }
        public void AddSecondaryAddress(Address Address)
        {
            AddressContacts.Add(new ClientAddress<SecondaryContact>() { Client = this, Value = Address });
        }

    }
}
