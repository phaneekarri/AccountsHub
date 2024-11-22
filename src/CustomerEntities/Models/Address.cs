
using System;
using InfraEntities;
using InfraEntities.Interfaces;

namespace CustomerEntities.Models
{
    public class Address : AuditableSoftDeleteEntity, IAuditEntity, ISoftDelete, IEquatable<Address>
    {
        public int Id { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }

        public bool Equals(Address other)
        {
            if (other == null) return false;

            return 
                this.AddressLine1 == other.AddressLine1 &&
                this.AddressLine2 == other.AddressLine2 &&
                this.City == other.City &&
                this.State == other.State &&
                this.PostalCode == other.PostalCode;
        }

        public override bool Equals(object obj)
        {
            if (obj is Address other)
            {
                return Equals(other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AddressLine1, AddressLine2, City, State, PostalCode);
        }
    }
}
