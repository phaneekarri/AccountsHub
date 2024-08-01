namespace InfraEntities.ModelType;

    public abstract class ModelType
    {
        public int Id { get; init; }
        public string? Description { get;  init; }
        public bool IsActive {get; set;} = true;
    }
