using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CustomerEntities.Interfaces;
using InfraEntities.Interfaces;

namespace CustomerEntities.Models;
    public static class ClientContactsExtensions
    {

        private static Predicate<IHasPriorityOrder> primaryCheck = (t) => t.PriorityOrder == 1;
        private static Func<IHasPriorityOrder, int, bool> PriorityCheck = (t, p) => t.PriorityOrder == p;
        public static IHasAsset<TContactInfo> GetPrimary<TContact, TContactInfo>(this ICollection<TContact> contacts, 
            Client client) where TContact : IHasClientRelation, IHasPriorityOrder , IHasAsset<TContactInfo>
        {
            var primary =   contacts.Single(c => c.ClientId == client.Id && primaryCheck(c));
            return primary;
        }
        
        public static IReadOnlyCollection<TContact> GetSecondary<TContact, TContactInfo>(this ICollection<TContact> contacts, 
        Client client, TContactInfo primaryInfo) where TContact : IHasClientRelation, IHasPriorityOrder => 
            contacts.Where(c => c.ClientId == client.Id && !primaryCheck(c)).ToList().AsReadOnly();            
        
        public static void SetAsPrimary<T, TAsset>(
            this ICollection<T> collection, 
            Client client, TAsset primaryInfo)
         where T : IHasClientRelation, IHasPriorityOrder, IHasAsset<TAsset>, new()
        {
            var existingPrimary = collection.FirstOrDefault(c => primaryCheck(c) && c.ClientId == client.Id );
            
            if(existingPrimary != null )
            {
                if(primaryInfo.Equals(existingPrimary.Value)) return;
                existingPrimary.PriorityOrder ++;
            }
            if(collection == null) collection = new  Collection<T>();
            var primary =  new T {  PriorityOrder = 2 };
            primary.Update(client);
            primary.Update(primaryInfo);
            collection.Add(primary);

        }

        public static void AddSecondary<T, TAsset>(
            this ICollection<T> collection, 
            Client client,  TAsset secondaryInfo)
        where T : IHasClientRelation, IHasPriorityOrder, IHasAsset<TAsset>, new()
        {
            if (collection.Any(c => c.Value != null && c.Value.Equals(secondaryInfo)))
            {
                throw new InvalidOperationException("Already exists.");
            }
            if(collection == null) collection = new  Collection<T>();
            var newItem = new T {  PriorityOrder = 2 };
            newItem.Update(secondaryInfo);
            newItem.Update(client);
            collection.Add(newItem);
        }

        public static void DeleteContact<T,  TAsset>(this ICollection<T> collection, Client client, TAsset value)
        where 
        T : IHasClientRelation, IHasAsset<TAsset>, IHasPriorityOrder
        {
            
            if (value == null) throw new ArgumentNullException(nameof(value));

            var item = collection.FirstOrDefault(c =>
            c.Value != null && c.Value.Equals(value) && c.ClientId == client.Id);
            if (primaryCheck(item))
            {
                throw new InvalidOperationException("Cannot delete a primary.");
            }

            if (item != null)            
            collection.Remove(item);

        }

    } 