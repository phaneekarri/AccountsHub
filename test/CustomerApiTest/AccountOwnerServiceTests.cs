using CustomerApi;
using CustomerApi.Dto;
using Microsoft.Extensions.Logging;
using Moq;

namespace CustomerApiTest;

public class AccountOwnerServiceTests : ServiceTests<AccountService>
{
   private AccountService? _accountService ;
   private ClientService? _clientService;
       
    protected override AccountService SetSUT()
    {
        _accountService = new AccountService(new Mock<ILogger<AccountService>>().Object, Mapper, Context);
         _clientService = new ClientService(new Mock<ILogger<ClientService>>().Object, Mapper, Context);

        return 
         new AccountService(
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
            Id = await _clientService!.Create(new CreateClient { FirstName = "Test", LastName = "Test", DOB = new DateOnly(2000,9,12)})
        };
        var accountId = await _accountService!.Create(new CreateAccount { Title = "Test title"});
            
        // Act    
        var result =  await SUT.AddOwners(accountId, new List<CreateAccountOwner>{setUp});

        //Assert
        
        Assert.That(result == true, "Failed saving");
       
        var savedAccount = Context.AccountOwners.Where(x => x.AccountId == 1 && x.ClientId == 2).ToList();
        Assert.NotNull(savedAccount != null || savedAccount?.Count > 0, "AccountOwner with given details are not saved");

    }
    
    [Test]
    public async Task Verify_CreateAccountOwners_EmptyOwners()
    {
        // Arrange 
        var accountId = await _accountService!.Create(new CreateAccount { Title = "Test title"});
            
        // Act    
       Assert.ThrowsAsync<ArgumentNullException>(async() => await SUT.AddOwners(accountId, null));

        //Assert
    }

    [Test]
    public async Task Verify_DeleteAccountOwnersByClient()
    {
        //Arrange
        var clientId = await _clientService!.Create(new CreateClient { FirstName = "Test", LastName = "Test", DOB = new DateOnly(2000,9,12)});
        var accountId = await _accountService!.Create(new CreateAccount { Title = "Test title"});
         var setUp = new CreateAccountOwner { 
            Id = clientId
        };
        var Owners = await SUT.AddOwners(accountId, new List<CreateAccountOwner>{setUp});
        //Act
         var result = await SUT.DeleteOwner(accountId, clientId);
        //Assert
        Assert.IsTrue(result, "Delete Account owners by client failed");
    }

        [Test]
    public async Task Verify_DeleteAccountOwnersByAccountId()
    {
        //Arrange
        var clientId = await _clientService!.Create(new CreateClient { FirstName = "Test", LastName = "Test", DOB = new DateOnly(2000,9,12)});
        var accountId = await _accountService!.Create(new CreateAccount { Title = "Test title"});
         var setUp = new CreateAccountOwner { 
            Id = clientId
        };
        var Owners = await SUT.AddOwners(accountId, new List<CreateAccountOwner>{setUp});
        //Act
         var result = await SUT.DeleteOwners(accountId);
        //Assert
        Assert.IsTrue(result, "Delete Account owners by client failed");
        
    }

    [Test]
    public async Task Verify_DeleteAccountOwnersByAccountId_Throws_KeynotfoundException()
    {
        //Arrange

        //Act
         Assert.ThrowsAsync<KeyNotFoundException>(async () => await SUT.DeleteOwners(1));
        //Assert
    }

    [Test]
    public async Task Verify_DeleteAccountOwnersByClietId_Throws_KeynotfoundException()
    {
        //Arrange

        //Act
        Assert.ThrowsAsync<KeyNotFoundException>(async () =>await SUT.DeleteOwner(1,1));
        //Assert
    }
}
