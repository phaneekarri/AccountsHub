using Microsoft.EntityFrameworkCore;

namespace UserAuthApi.Services;

public class BaseService<T, TDBContext>
where  TDBContext : DbContext
{
    protected readonly ILogger<T> Logger;
    protected readonly TDBContext Context;
    public BaseService(ILogger<T> logger,
     TDBContext context)
    {
        Logger = logger;
        Context = context;
    }
}
