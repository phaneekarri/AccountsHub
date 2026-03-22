using System;
using CustomerEntities;
using Infra;
using InfraEntities.ModelType;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Moq;

namespace CustomerEntitiesTest
{
    public static class TestExtension
    {
        public static void IsPrimary(this ModelType testObject)
        {
            Assert.Equal(1, testObject.Id);
            Assert.Equal("Primary", testObject.Description);
        }
        public static void IsSecondary(this ModelType testObject)
        {
            Assert.Equal(2, testObject.Id);
            Assert.Equal("Secondary", testObject.Description);
        }

        public static CustomerDbContext GetTestDbContext()
        {            
            var options = 
                new DbContextOptionsBuilder<CustomerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
                var userResolverMoq = new Mock<IUserResolver>();
                 userResolverMoq.Setup( r => r.Get()).Returns("test_user");
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
