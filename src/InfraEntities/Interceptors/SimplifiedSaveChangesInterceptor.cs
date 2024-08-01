using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace InfraEntities.Interceptors;

public abstract class SimplifiedSaveChangesInterceptor<T> : SaveChangesInterceptor
 where T : class
{

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if(eventData == null) return result;
        if(eventData.Context != null) 
        foreach (var entry in eventData.Context.ChangeTracker.Entries<T>())        
            HandleInterception(entry);     
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if(eventData == null) return result;
        if(eventData.Context != null) 
        foreach (var entry in eventData.Context.ChangeTracker.Entries<T>())        
            HandleInterception(entry); 
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public abstract void HandleInterception(EntityEntry<T> entry);   
}


public class CommonInterceptor<T>(Action<EntityEntry<T>> Interception) : SimplifiedSaveChangesInterceptor<T>
where T : class
{
    public override void HandleInterception(EntityEntry<T> entry)
    {
       Interception.Invoke(entry);
    }
}

