using System;
using System.Runtime.CompilerServices;

namespace CustomerEntities.Models
{
    public class Address : AuditEntity, ISoftDelete
    {
        public int Id { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
         public bool IsDeleted { get; set; }
    }
}
