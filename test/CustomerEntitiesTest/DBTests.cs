using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CustomerEntitiesTest;

[TestClass]
public abstract class DBTests<TContext> where TContext : DbContext 
{
  protected TContext context;
  protected IDbContextTransaction transaction;

    protected abstract TContext CreateDBContext();

    [TestInitialize]
    public void Initialize()
    {
        context = CreateDBContext();
       // transaction = context.Database.BeginTransaction();
        context.Database.EnsureCreated();
    }

    [TestCleanup]
    public void cleanup()
    {
        //transaction.Rollback();
        //transaction.Dispose();
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}
