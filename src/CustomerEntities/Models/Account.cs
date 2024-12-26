using System;
using System.Collections.Generic;
using System.Linq;
using InfraEntities;
using InfraEntities.Interfaces;

namespace CustomerEntities.Models
{
    public class Account : AuditableSoftDeleteEntity, 
    IAuditEntity, 
    ISoftDelete   
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AccountNumber {get; set;}
        public ICollection<AccountOwner> AccountOwners { get; set; } = new HashSet<AccountOwner>();

        public void AddOwners(IEnumerable<Client> owners)
        {
            if(owners.Count(x => x.Id <= 0) > 0) throw new Exception("One or more owners are not allowed");
            AccountOwners =
            AccountOwners.UnionBy(owners.Select(x => {
                var owner = new AccountOwner();
                owner.Update(this);
                owner.Update(x);
                return owner;
            }), (x) => (x.AccountId , x.ClientId, x.PriorityOrder))
            .ToList();
        }

        public void DeleteOwner(int clientId){
           var ownerToDelete = AccountOwners.FirstOrDefault(x => x.ClientId == clientId);
            if(ownerToDelete != null)
            {
                AccountOwners.Remove(ownerToDelete);
            }
            else throw new KeyNotFoundException("No matching item found");
        }

        public void DeleteAllOwners(){
            AccountOwners = [];
        }
    }
}
