public interface IUpdateEntity<T>{
    void Update(T newValue);
 }

 public interface IUpdateEntity<TOut, T>{
    TOut Update(T newValue);
 }