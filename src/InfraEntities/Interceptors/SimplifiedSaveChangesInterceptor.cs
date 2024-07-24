using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace InfraEntities.Interceptors;

public abstract class SimplifiedSaveChangesInterceptor : SaveChangesInterceptor
{

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if(eventData == null) return result;
        if(eventData.Context != null) HandleInterception(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if(eventData == null) return result;
        if(eventData.Context != null)  HandleInterception(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public abstract void HandleInterception(DbContext context);   
}



