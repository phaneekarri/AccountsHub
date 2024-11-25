namespace InfraEntities.Interfaces;
public interface IModelType
{
    int Id { get; }
    string Name {get; }
    string? Description { get;  }
    bool IsActive {get;}
}

public interface IHasDisplayOrder
{
    int DisplayOrder {get;}
}

public interface IHasPriorityOrder
{
    int PriorityOrder {get; set;}
}