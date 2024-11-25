using InfraEntities.Interfaces;

namespace InfraEntities;

public class AuditableSoftDeleteEntity :
 AuditEntity
,ISoftDelete
{
   private bool _IsDeleted = false;
   public bool IsDeleted {get => _IsDeleted; init { _IsDeleted = value;}}
   public void MarkDeleted() => _IsDeleted = true;
}
