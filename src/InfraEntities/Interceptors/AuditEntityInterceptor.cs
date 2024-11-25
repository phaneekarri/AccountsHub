using InfraEntities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InfraEntities.Interceptors;

public class AuditEntityInterceptor : SimplifiedSaveChangesInterceptor<AuditEntity>
{ 
    string _user ;
    public AuditEntityInterceptor(string changesBy)
    {
       _user = changesBy;
    }
    public override void HandleInterception(EntityEntry<AuditEntity> entry)
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