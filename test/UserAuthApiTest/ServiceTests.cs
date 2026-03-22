using UserAuthApi;
using UserAuthEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace UserAuthApiTest;

[TestFixture]
public abstract class ServiceTests<T>
{
     protected T SUT;
    protected AuthDBContext Context;
    protected Mock<ILogger<T>> LoggerMock;
    

    [SetUp]
    protected virtual void SetUp()
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

    [TearDown]
    public void TearDown()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}