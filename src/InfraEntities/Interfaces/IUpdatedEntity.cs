namespace InfraEntities.Interfaces;

public interface IUpdatedEntity 
{
   DateTime UpdatedAt {get;}
   string? UpdatedBy {get;} 
}