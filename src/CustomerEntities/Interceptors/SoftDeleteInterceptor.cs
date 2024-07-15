using System;
using Microsoft.EntityFrameworkCore;


namespace CustomerEntities;

public class SoftDeleteInterceptor : SimplifiedSaveChangesInterceptor
{
    public override void HandleInterception(DbContext context)
    {
         foreach (var entry in context.ChangeTracker.Entries<ISoftDelete>())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.DeletedAt = DateTimeOffset.Now;
            }
        }
    }
}