
namespace InfraEntities.Interfaces;

public interface ISoftDelete
{
   bool IsDeleted {get;}
   void MarkDeleted();
}
