using System;
using System.Linq;
using CustomerEntities;
using CustomerEntities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CustomerEntitiesTest;

[TestClass]
public class CustomerDbContextTests
{


    [TestMethod]
    public void CanAddAndRetrieveYourModel()
    {
        
        using (var context = TestExtension.GetTestDbContext())
        {
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            context.Accounts.Add(new Account { Id = 1, Title = "TestName" });
            context.SaveChanges();
        }

        // Act
        using (var context = TestExtension.GetTestDbContext())
        {
            var model = context.Accounts.FirstOrDefault(m => m.Title == "TestName");
            // Assert
            Assert.IsNotNull(model);
            Assert.AreEqual("TestName", model.Title);
        }
    }
}
