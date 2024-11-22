using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CustomerEntities.Models.Contacts;
using CustomerEntities.Models.Types;

namespace CustomerEntities.Models
{
    public class ClientContact<T> : Contact<T>
    {
        public ClientContact(){}
        public ClientContact(ClientContact<T> copy)
        {
             if(copy != null)
             {
                this.Client = copy.Client;
                this.ContactType = copy.ContactType;
                this.Value = copy.Value;                
             }
        }

        private Client _client;
        public Client Client 
        { 
            get => _client;
            set
            {
                _client = value;
                _clientId = value.Id;
            }
        }

        private int _clientId;
        public int ClientId 
        { 
            get => _clientId;
            set 
            {
               _clientId = value;
               _client = null;
            }
        }

       public bool IsPrimary => ContactType.Description == "Primary";

    }

    public static class ClientContactsExtensions
    {
        public static ClientContact<TContactInfo> GetPrimary<TContact, TContactInfo>(this ICollection<TContact> contacts, 
            Client client, TContactInfo primaryInfo) where TContact : ClientContact<TContactInfo> 
            {
               var primary =   contacts.Single(c => c.ClientId == client.Id && c.IsPrimary);
               return new ClientContact<TContactInfo>(primary);
            }
        public static IReadOnlyCollection<TContact> GetSecondary<TContact, TContactInfo>(this ICollection<TContact> contacts, 
        Client client, TContactInfo primaryInfo) where TContact : ClientContact<TContactInfo> => contacts.Where(c => c.ClientId == client.Id && c.IsPrimary).ToList().AsReadOnly();            
        public static void SetAsPrimary<TContact, TContactInfo>(
            this ICollection<TContact> contacts, 
            Client client, TContactInfo primaryInfo)
         where TContact : ClientContact<TContactInfo>, new()
        {
            var existingPrimary = contacts?.FirstOrDefault(c => c.IsPrimary && c.ClientId == client.Id );
            
            if(existingPrimary != null )
            {
                if(primaryInfo.Equals(existingPrimary.Value)) return;
                existingPrimary.ContactType = new SecondaryContact();
            }
            if(contacts == null) contacts = new  Collection<TContact>();
            var PrimaryContact =  new TContact
            { 
                Client = client, 
                Value = primaryInfo,
                ContactType = new PrimaryContact()                      
            };
            contacts.Add(PrimaryContact);

        }

        public static void AddSecondary<TContact, TContactInfo>(
            this ICollection<TContact> contacts, 
            Client client,  TContactInfo secondaryInfo)
        where TContact : ClientContact<TContactInfo>, new()
        {
            if (contacts.Any(c => c.Value != null && c.Value.Equals(secondaryInfo)))
            {
                throw new InvalidOperationException("A contact with the same information already exists.");
            }
            if(contacts == null) contacts = new  Collection<TContact>();
            contacts.Add(new TContact { Client = client, Value = secondaryInfo, ContactType = new SecondaryContact() });
        }

        public static void DeleteContact<TContact, TContactInfo>(
            this ICollection<TContact> contacts, 
            Client client, TContactInfo value
        )where TContact : ClientContact<TContactInfo>, new()
        {
            
            if (contacts == null) throw new ArgumentNullException(nameof(contacts));
            if (value == null) throw new ArgumentNullException(nameof(value));

            var contactToRemove = contacts.FirstOrDefault(c =>
            c.Value != null && c.Value.Equals(value) && c.ClientId == client.Id);
            if (contactToRemove.IsPrimary)
            {
                throw new InvalidOperationException("Cannot delete a primary contact.");
            }

            if (contactToRemove != null)            
            contacts.Remove(contactToRemove);

        }

    } 
}
