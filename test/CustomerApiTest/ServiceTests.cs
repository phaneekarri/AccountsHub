using AutoMapper;
using CustomerApi;
using CustomerEntities;
using CustomerEntitiesTest;
using Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace CustomerApiTest;

[TestFixture]
public abstract class ServiceTests<T>
{
     protected T SUT;
    protected CustomerDbContext Context;
    protected Mock<ILogger<T>> LoggerMock;
    
    protected IMapper Mapper;


    [SetUp]
    protected virtual void SetUp()
    {
        LoggerMock = new Mock<ILogger<T>>();
        MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
        Mapper = mapperConfiguration.CreateMapper();
        var options = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Use in-memory database for testing
                .Options;
        var mockUserResolver = new Mock<IUserResolver>();
        mockUserResolver.Setup(c => c.Get()).Returns("Test_User");
        Context = TestExtension.GetTestDbContext();        
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
