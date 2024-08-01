using Castle.Core.Logging;
using CustomerApi;
using CustomerApi.Dto;
using CustomerEntities.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace CustomerApiTest;

public class AccountOwnerServiceTests : ServiceTests<AccountOwnerService>
{
   private AccountService _accountService ;
   private ClientService _clientService;
       
    protected override AccountOwnerService SetSUT()
    { 
        _accountService = new AccountService(new Mock<ILogger<AccountService>>().Object, Mapper, Context);
         _clientService = new ClientService(new Mock<ILogger<ClientService>>().Object, Mapper, Context);

        return 
         new AccountOwnerService(
            LoggerMock.Object,
            Mapper,
            Context        
        );         
    }
    
    [Test]
    public async Task Verify_CreateAccountOwners()
    {
        // Arrange 
        var setUp = new CreateAccountOwner { 
            AccountOwnerType = 1, 
            ClientId = await _clientService.Create(new CreateClient { FirstName = "Test", LastName = "Test", DOB = new DateOnly(2000,9,12)})
        };
        var accountId = await _accountService.Create(new CreateAccount { Title = "Test title"});
            
        // Act    
        var result =  await SUT.CreateOwnersByAccount(accountId, new List<CreateAccountOwner>{setUp});

        //Assert
        
        Assert.That(result == true, "Failed saving");
       
        var savedAccount = Context.AccountOwners.Where(x => x.AccountId == 1 && x.AccountOwnerTypeId ==1 && x.ClientId == 2).ToList();
        Assert.NotNull(savedAccount != null || savedAccount?.Count > 0, "AccountOwner with given details are not saved");

    }
    
    [Test]
    public async Task Verify_CreateAccountOwners_EmptyOwners()
    {
        // Arrange 
        var accountId = await _accountService.Create(new CreateAccount { Title = "Test title"});
            
        // Act    
       Assert.ThrowsAsync<ArgumentNullException>(async() => await SUT.CreateOwnersByAccount(accountId, null));

        //Assert
    }

    [Test]
    public async Task Verify_DeleteAccountOwnersByClient()
    {
        //Arrange
        var clientId = await _clientService.Create(new CreateClient { FirstName = "Test", LastName = "Test", DOB = new DateOnly(2000,9,12)});
        var accountId = await _accountService.Create(new CreateAccount { Title = "Test title"});
         var setUp = new CreateAccountOwner { 
            AccountOwnerType = 1, 
            ClientId = clientId
        };
        var Owners = await SUT.CreateOwnersByAccount(accountId, new List<CreateAccountOwner>{setUp});
        //Act
         var result = await SUT.DeleteAccountOwnersByClient(accountId, clientId);
        //Assert
        Assert.IsTrue(result, "Delete Account owners by client failed");
    }

        [Test]
    public async Task Verify_DeleteAccountOwnersByAccountId()
    {
        //Arrange
        var clientId = await _clientService.Create(new CreateClient { FirstName = "Test", LastName = "Test", DOB = new DateOnly(2000,9,12)});
        var accountId = await _accountService.Create(new CreateAccount { Title = "Test title"});
         var setUp = new CreateAccountOwner { 
            AccountOwnerType = 1, 
            ClientId = clientId
        };
        var Owners = await SUT.CreateOwnersByAccount(accountId, new List<CreateAccountOwner>{setUp});
        //Act
         var result = await SUT.DeleteAccountOwners(accountId);
        //Assert
        Assert.IsTrue(result, "Delete Account owners by client failed");
        
    }

    [Test]
    public async Task Verify_DeleteAccountOwnersByAccountId_Throws_KeynotfoundException()
    {
        //Arrange

        //Act
         Assert.ThrowsAsync<KeyNotFoundException>(async () => await SUT.DeleteAccountOwners(1));
        //Assert
    }

    [Test]
    public async Task Verify_DeleteAccountOwnersByClietId_Throws_KeynotfoundException()
    {
        //Arrange

        //Act
        Assert.ThrowsAsync<KeyNotFoundException>(async () =>await SUT.DeleteAccountOwnersByClient(1,1));
        //Assert
    }
}
