using UserAuthApi;
using UserAuthEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace UserAuthApiTest;

public abstract class ServiceTests<T> : IDisposable
{
     protected T SUT;
    protected AuthDBContext Context;
    protected Mock<ILogger<T>> LoggerMock;
    

    protected ServiceTests()
    {
        LoggerMock = new Mock<ILogger<T>>();
        var options = new DbContextOptionsBuilder<AuthDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        Context = new AuthDBContext(options);        
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