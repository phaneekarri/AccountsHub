using LoanEntities.Models.Contacts;
using LoanEntities.Models.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoanEntitiesTest
{
    [TestClass]
    public class ClientContactsTest
    {
        [TestMethod]
        public void ClientPhone_Tests()
        {
            new ClientPhone<PrimaryContact>().ContactType.IsPrimary();
            new ClientPhone<SecondaryContact>().ContactType.IsSecondary();
        }

        [TestMethod]
        public void ClientEmail_Tests()
        {
            new ClientEmail<PrimaryContact>().ContactType.IsPrimary();
            new ClientEmail<SecondaryContact>().ContactType.IsSecondary();
        }
        [TestMethod]
        public void ClientAddress_Tests()
        {
            new ClientAddress<PrimaryContact>().ContactType.IsPrimary();
            new ClientAddress<SecondaryContact>().ContactType.IsSecondary();
        }
    }
}
