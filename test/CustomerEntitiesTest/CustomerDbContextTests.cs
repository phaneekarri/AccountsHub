using System.Linq;
using CustomerEntities;
using CustomerEntities.Models;

namespace CustomerEntitiesTest;

public class CustomerDbContextTests : DBTests<CustomerDbContext>
{
    protected override CustomerDbContext CreateDBContext() => TestExtension.GetTestDbContext();
    [Fact]
    public void CanAddAndRetrieveYourModel()
    {       //Arrange
            context.Accounts.Add(new Account { Title = "TestName" });
            context.SaveChanges();
            //Act
            var model = context.Accounts.FirstOrDefault(m => m.Title == "TestName");
            // Assert
            Assert.NotNull(model);
            Assert.Equal("TestName", model.Title);
    }

}
