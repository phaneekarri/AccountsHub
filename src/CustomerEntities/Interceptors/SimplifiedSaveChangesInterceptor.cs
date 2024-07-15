using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CustomerEntities;

public abstract class SimplifiedSaveChangesInterceptor : SaveChangesInterceptor
{

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        HandleInterception(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        HandleInterception(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public abstract void HandleInterception(DbContext context);   
}



