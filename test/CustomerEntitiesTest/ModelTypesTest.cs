using InfraEntities.ModelType;
using CustomerEntities.Models.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CustomerEntitiesTest
{
    [TestClass]
    public class ModelTypesTest
    {
        [TestMethod]
        public void ModelType_Primary_Test()
        {
            new ModelType<Primary>().IsPrimary();
        }

        [TestMethod]
        public void ModelType_Secondary_Test()
        {
            new ModelType<Secondary>().IsSecondary();
        }

        [TestMethod]
        public void ContactType_Test()
        {
            new PrimaryContact().IsPrimary();
            new SecondaryContact().IsSecondary();
        }
        [TestMethod]
        public void AccountOwner_Test()
        {
            new PrimaryAccountOwner().IsPrimary();
            new SecondaryAccountOwner().IsSecondary();
        }

    }
}
