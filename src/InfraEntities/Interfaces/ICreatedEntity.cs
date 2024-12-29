namespace InfraEntities.Interfaces;

public interface ICreatedEntity
{
   DateTime CreatedAt {get;}
   string? CreatedBy {get;}
}