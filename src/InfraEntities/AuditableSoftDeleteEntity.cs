using InfraEntities.Interfaces;

namespace InfraEntities;

public class AuditableSoftDeleteEntity : AuditEntity, IAuditEntity, ISoftDelete
{
   public bool IsDeleted {get; set;}
}
