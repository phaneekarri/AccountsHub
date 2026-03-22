using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace CustomerEntitiesTest;

public abstract class DBTests<TContext> : IDisposable where TContext : DbContext 
{
  protected TContext context;
  protected IDbContextTransaction transaction;

    protected abstract TContext CreateDBContext();

    public DBTests()
    {
        context = CreateDBContext();
       // transaction = context.Database.BeginTransaction();
        context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        //transaction.Rollback();
        //transaction.Dispose();
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}
