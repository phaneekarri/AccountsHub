using System;
using Microsoft.EntityFrameworkCore;

namespace CustomerEntities;
public class AuditEntityInterceptor : SimplifiedSaveChangesInterceptor
{ 
    string _user ;
    public AuditEntityInterceptor(string changesBy)
    {
       _user = changesBy;
    }
    public override void HandleInterception(DbContext context)
    {
        foreach (var entry in context.ChangeTracker.Entries<AuditEntity>())
            {
                    if (entry.State == EntityState.Added){

                        entry.Entity.CreatedAt = DateTime.Now;
                        entry.Entity.CreatedBy = _user;
                        entry.Entity.UpdatedAt = DateTime.Now;
                        entry.Entity.UpdatedBy = _user;
                    }
                    if(entry.State == EntityState.Modified) {
                        entry.Entity.UpdatedAt = DateTime.Now;
                        entry.Entity.UpdatedBy = _user;
                    }                
           }
    }
}