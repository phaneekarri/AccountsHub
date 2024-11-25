using InfraEntities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace InfraEntities.Interceptors;

public class SoftDeleteInterceptor : SimplifiedSaveChangesInterceptor<ISoftDelete>
{
    public override void HandleInterception(EntityEntry<ISoftDelete> entry) 
    {
        if (entry.State == EntityState.Deleted)
        {
                entry.State = EntityState.Modified;
                entry.Entity.MarkDeleted();            
        }
    }
}