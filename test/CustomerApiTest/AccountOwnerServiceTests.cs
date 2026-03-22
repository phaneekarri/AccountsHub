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
        _accountService = new AccountService(Context);
         _clientService = new ClientService(Context);

        return 
         new AccountService(
            Context);         
    }
    
    [Fact]
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
        
        Assert.True(result, "Failed saving");
       
        var savedAccount = Context.AccountOwners.Where(x => x.AccountId == accountId && x.ClientId == setUp.Id).ToList();
        Assert.True(savedAccount.Count > 0, "AccountOwner with given details are not saved");

    }
    
    [Fact]
    public async Task Verify_CreateAccountOwners_EmptyOwners()
    {
        // Arrange 
        var accountId = await _accountService!.Create(new CreateAccount { Title = "Test title"});
            
        // Act    
       await Assert.ThrowsAsync<ArgumentNullException>(async() => await SUT.AddOwners(accountId, null));

        //Assert
    }

    [Fact]
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
        Assert.True(result, "Delete Account owners by client failed");
    }

        [Fact]
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
        Assert.True(result, "Delete Account owners by accountId failed");
        
    }

    [Fact]
    public async Task Verify_DeleteAccountOwnersByAccountId_Throws_KeynotfoundException()
    {
        //Arrange

        //Act
         await Assert.ThrowsAsync<KeyNotFoundException>(async () => await SUT.DeleteOwners(1));
        //Assert
    }

    [Fact]
    public async Task Verify_DeleteAccountOwnersByClientId_Throws_KeyNotFoundException()
    {
        //Arrange

        //Act
        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>await SUT.DeleteOwner(1,1));
        //Assert
    }
}
