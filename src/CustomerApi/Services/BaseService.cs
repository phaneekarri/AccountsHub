using AutoMapper;
using CustomerEntities;
using Microsoft.Extensions.Logging;

namespace CustomerApi;

public class BaseService<T>
{

    protected readonly ILogger<T> Logger;
    protected readonly CustomerDbContext Context;
    protected readonly IMapper Mapper;
    public BaseService(ILogger<T> logger, IMapper mapper, CustomerDbContext context)
    {
        Logger = logger;
        Context = context;
        Mapper = mapper;
    }
}

