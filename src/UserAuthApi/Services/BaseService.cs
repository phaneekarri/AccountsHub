using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace UserAuthApi.Services;

public class BaseService<T, TDBContext>
where  TDBContext : DbContext
{
    protected readonly ILogger<T> Logger;
    protected readonly TDBContext Context;
    protected readonly IMapper Mapper;
    public BaseService(ILogger<T> logger, IMapper mapper,
     TDBContext context)
    {
        Logger = logger;
        Context = context;
        Mapper = mapper;
    }
}
