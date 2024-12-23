using InfraEntities.Interfaces;

namespace InfraEntities.ModelType;
    public abstract class ModelType : IModelType
    {
        public int Id { get; init; }
        public string Name {get; init;} = "";
        public string? Description { get;  init; }
        public bool IsActive {get; set;} = true;
    }
