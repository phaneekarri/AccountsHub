namespace InfraEntities.Interfaces;

public interface IAuditEntity
{
   DateTime CreatedAt {get; set;}
   string? CreatedBy {get; set;}
   DateTime UpdatedAt {get; set;}
   string? UpdatedBy {get; set;} 
}
