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
        public IReadOnlyCollection<ClientEmailContact> EmailContacts => _emailContacts.AsReadOnly();
        public ClientEmailContact PrimaryEmail => _emailContacts.SingleOrDefault(x => x.IsPrimary);        
        public IReadOnlyCollection<ClientEmailContact> SecondaryEmails => _emailContacts.Where(x => !x.IsPrimary).ToList().AsReadOnly();                
        public void SetAsPrimaryEmail(string Email ) => _emailContacts.SetAsPrimary(this, Email);
        public void AddSecondaryEmail(string Email) => _emailContacts.AddSecondary(this, Email);        
        public void DeleteEmail(string Email) => _emailContacts.DeleteContact(this, Email);

        private List<ClientPhoneContact> _phoneContacts = new();
        public IReadOnlyCollection<ClientPhoneContact> PhoneContacts { get; set; } = new List<ClientPhoneContact>();        
        public ClientPhoneContact PrimaryPhone => _phoneContacts.SingleOrDefault(x => x.IsPrimary);        
        public IReadOnlyCollection<ClientPhoneContact> SecondaryPhones => _phoneContacts.Where(x => !x.IsPrimary).ToList().AsReadOnly();  
        public void SetAsPrimaryPhone(string Phone) => _phoneContacts.SetAsPrimary(this, Phone);                
        public void AddSecondaryPhone(string Phone) => _phoneContacts.AddSecondary(this, Phone);      
        public void DeletePhone(string Phone) => _phoneContacts.DeleteContact(this, Phone);

        private List<ClientAddressContact> _addressContacts = new();
        public IReadOnlyCollection<ClientAddressContact> AddressContacts { get; set; } = new HashSet<ClientAddressContact>();        
        public ClientAddressContact PrimaryAddress => AddressContacts.SingleOrDefault(x => x.IsPrimary);
        
        public IReadOnlyCollection<ClientAddressContact> SecondaryAddresses => AddressContacts.Where(x => !x.IsPrimary).ToList().AsReadOnly();
             
        public void SetAsPrimaryAddress(Address Address) => _addressContacts.SetAsPrimary(this, Address);
        public void AddSecondaryAddress(Address Address) => _addressContacts.AddSecondary(this, Address);
        public void DeleteEmail(Address Address) => _addressContacts.DeleteContact(this, Address);

       
    }
}
