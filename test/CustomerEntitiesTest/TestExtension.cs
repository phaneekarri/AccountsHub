using CustomerEntities;
using CustomerEntities.Models.Types;
using Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CustomerEntitiesTest
{
    public static class TestExtension
    {
        public static void IsPrimary(this ModelType testObject)
        {
            Assert.AreEqual(testObject.Id, 1);
            Assert.AreEqual(testObject.Description, "Primary");
        }
        public static void IsSecondary(this ModelType testObject)
        {
            Assert.AreEqual(testObject.Id, 2);
            Assert.AreEqual(testObject.Description, "Secondary");
        }

        public static CustomerDbContext GetTestDbContext()
        {            
            var options = 
                new DbContextOptionsBuilder<CustomerDbContext>()
                .UseSqlite("Data Source=mydatabase.db")
                .Options;
                var userResolverMoq = new Mock<IUserResolver>();
                 userResolverMoq.Setup( r => r.Get()).Returns("test_user");
            return new CustomerDbContext(options, userResolverMoq.Object);
        }

        public static ModelBuilder GetTestModelBuilder()
        {
            var testdBContext = GetTestDbContext();
            // Get the convention set for this db
            var conventionSet = ConventionSet.CreateConventionSet(testdBContext);

            // Now create the ModelBuilder
            return new ModelBuilder(conventionSet);
        }
    }
}
