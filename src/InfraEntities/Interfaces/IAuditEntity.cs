namespace InfraEntities.Interfaces;

public interface IAuditEntity
{
   DateTime CreatedAt {get;}
   string? CreatedBy {get;}
   DateTime UpdatedAt {get;}
   string? UpdatedBy {get;} 
}
