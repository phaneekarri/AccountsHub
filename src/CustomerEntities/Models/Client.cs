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
        
        public int Age 
        { 
            get 
            {
                var age = DateTime.Today.Year - DOB.Year;
                if (DateTime.Today.Day > DOB.Day) age++;
                return age;
            }            
        }
        
        public ICollection<AccountOwner> Accounts { get; set; }= new HashSet<AccountOwner>();
        
        private List<ClientEmailContact> _emailContacts = new();
        public IReadOnlyCollection<ClientEmailContact> EmailContacts { get; set; } = new HashSet<ClientEmailContact>();
        public ClientEmailContact PrimaryEmail => _emailContacts.SingleOrDefault(x => x.IsPrimary);        
        public ICollection<ClientEmailContact> SecondaryEmail => _emailContacts.Where(x => !x.IsPrimary).ToHashSet();                
        public void SetAsPrimaryEmail(string Email ) => _emailContacts.SetAsPrimary(this, Email);
        public void AddSecondaryEmail(string Email) => _emailContacts.AddSecondary(this, Email);        
        public void DeleteEmail(string Email) => _emailContacts.DeleteContact(this, Email);

        public ICollection<ClientPhoneContact> PhoneContacts { get; set; } = new HashSet<ClientPhoneContact>();        
        public ClientPhoneContact PrimaryPhone => PhoneContacts.SingleOrDefault(x => x.IsPrimary);        
        public ICollection<ClientPhoneContact> SecondaryPhone => PhoneContacts.Where(x => !x.IsPrimary).ToHashSet();                          
        public void SetAsPrimaryPhone(string Phone) => PhoneContacts.SetAsPrimary(this, Phone);                
        public void AddSecondaryPhone(string Phone) => PhoneContacts.AddSecondary(this, Phone);      
        public void DeletePhone(string Phone) => PhoneContacts.DeleteContact(this, Phone);

        public ICollection<ClientAddressContact> AddressContacts { get; set; } = new HashSet<ClientAddressContact>();        
        public ClientAddressContact PrimaryAddress => AddressContacts.SingleOrDefault(x => x.IsPrimary);
        
        public ICollection<ClientAddressContact> SecondaryAddresses => AddressContacts.Where(x => !x.IsPrimary).ToHashSet();
             
        public void SetAsPrimaryAddress(Address Address) => AddressContacts.SetAsPrimary(this, Address);
        public void AddSecondaryAddress(Address Address) => AddressContacts.AddSecondary(this, Address);
        public void DeleteEmail(Address Address) => AddressContacts.DeleteContact(this, Address);

       
    }
}
