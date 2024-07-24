using InfraEntities.Interfaces;

namespace InfraEntities;

public class AuditEntity: IAuditEntity
{
   public DateTime CreatedAt {get; set;}
   public string? CreatedBy {get; set;}
   public DateTime UpdatedAt {get; set;}
   public string? UpdatedBy {get; set;} 
}
