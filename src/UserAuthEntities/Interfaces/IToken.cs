namespace UserAuthEntities.Interfaces;

public interface IToken<T> : ICreated, IHasExpiry, ICanInvalidate
{
    public T Token {get;}
} 

public interface IUserToken<T> : IToken<T>
{
    Guid UserId {get;}
}