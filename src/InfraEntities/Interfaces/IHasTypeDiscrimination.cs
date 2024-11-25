namespace InfraEntities.Interfaces;
public interface IHasTypeDiscrimination<TDiscrimination> where TDiscrimination : IModelType
{
    TDiscrimination Type {get;}    
    bool IsType(Predicate<TDiscrimination> predicate);
}
