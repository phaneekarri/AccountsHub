
using InfraEntities;
using InfraEntities.Interfaces;

namespace CustomerEntities.Models
{
    public class Address : AuditableSoftDeleteEntity, IAuditEntity, ISoftDelete
    {
        public int Id { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }
}
