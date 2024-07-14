using Microsoft.AspNetCore.Http;

namespace Infra;

public interface IHttpResolver<T>{
    T Get ();
}
public abstract class HttpResolver<T>
{
   protected readonly IHttpContextAccessor Context;

   public HttpResolver(IHttpContextAccessor context)
   {
     Context = context;
   }
   public abstract T Get()
}

public interface IUserResolver : IHttpResolver<string?>{}

public class UserResolver : HttpResolver<string?> ,IUserResolver
{
    public UserResolver(IHttpContextAccessor context) : base(context){}
    public override string? Get() => Context?.HttpContext?.User?.Identity?.Name;
}