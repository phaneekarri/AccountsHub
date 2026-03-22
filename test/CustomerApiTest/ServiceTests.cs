using CustomerApi;
using CustomerEntities;
using CustomerEntitiesTest;
using Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace CustomerApiTest;

public abstract class ServiceTests<T> : IDisposable
{
     protected T SUT;
    protected CustomerDbContext Context;
    protected Mock<ILogger<T>> LoggerMock;
    

    protected ServiceTests()
    {
        LoggerMock = new Mock<ILogger<T>>();
        var options = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Use in-memory database for testing
                .Options;
        var mockUserResolver = new Mock<IUserResolver>();
        mockUserResolver.Setup(c => c.Get()).Returns("Test_User");
        Context = new CustomerDbContext(options);        
        Context.Database.EnsureCreated();
        SUT = SetSUT();
    }
    
    protected abstract T SetSUT();

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}
