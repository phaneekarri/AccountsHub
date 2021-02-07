using CustomerEntities;
using CustomerEntities.Models.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        public static DbContext GetTestDbContext()
        {
            var options = 
                new DbContextOptionsBuilder<CustomerDbContext>()
                .UseSqlServer(".")
                .Options;
            return new CustomerDbContext(options);
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
