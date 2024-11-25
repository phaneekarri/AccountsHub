namespace InfraEntities.Interfaces;

public interface IHasAsset<T>
{
    T Value {get;}
    void Update(T value);
}