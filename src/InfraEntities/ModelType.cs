namespace InfraEntities.ModelType;

    public abstract class ModelType
    {
        public int Id { get; protected set; }
        public string? Description { get; protected set; }

       protected virtual void  SetType<T>() where T : ModelType
        {
            T type = Activator.CreateInstance<T>();
            Id = type.Id;
            Description = type.Description;
        }
    }

    public class ModelType<T> : ModelType where T : ModelType
    {
        public ModelType()
        {
            SetType<T>();
        }

    }