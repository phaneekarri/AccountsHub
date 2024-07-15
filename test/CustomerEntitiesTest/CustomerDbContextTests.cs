using System.Linq;
using CustomerEntities;
using CustomerEntities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CustomerEntitiesTest;

[TestClass]
public class CustomerDbContextTests : DBTests<CustomerDbContext>
{
    protected override CustomerDbContext CreateDBContext() => TestExtension.GetTestDbContext();
    [TestMethod]
    public void CanAddAndRetrieveYourModel()
    {       //Arrange
            context.Accounts.Add(new Account { Title = "TestName" });
            context.SaveChanges();
            //Act
            var model = context.Accounts.FirstOrDefault(m => m.Title == "TestName");
            // Assert
            Assert.IsNotNull(model);
            Assert.AreEqual("TestName", model.Title);
    }

}
