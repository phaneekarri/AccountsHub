using InfraEntities.ModelType;

namespace CustomerEntities.Models.Types
{


    public class Primary : ModelType
    {
        public Primary()
        {
            Id = 1;
            Description = "Primary";
        }
    }

    public class Secondary : ModelType
    {
        public Secondary()
        {
            Id = 2;
            Description = "Secondary";
        }
    }
}
