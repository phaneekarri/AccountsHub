using System;

namespace InfraEntities;

public class AuditEntity
{
   public DateTime CreatedAt {get; set;}
   public string? CreatedBy {get; set;}
   public DateTime UpdatedAt {get; set;}
   public string? UpdatedBy {get; set;} 
}
