using CustomerApi;
using CustomerApi.Dto;

namespace CustomerApiTest;

public class ClientServiceTests : ServiceTests<ClientService>
{

    protected override ClientService SetSUT() => new ClientService(LoggerMock.Object, Mapper, Context);
    
    [Test]
    public async Task Verify_CreateClient()
    {
        //Arrange
        CreateClient setUp = new CreateClient { FirstName = "John", LastName = "Smith", DOB = new DateOnly(2001, 12, 22)};
        // Act
        var result = await SUT.Create(setUp);
        //Assert        
        Assert.That(result >0, "Client is not created");
    }

    [Test]
    public async Task Verify_UpdateClient()
    {
        //Arrange
        CreateClient client1 = new CreateClient { FirstName = "John", LastName = "Smith", DOB = new DateOnly(2001, 12, 22)};
        var createdClientId = await SUT.Create(client1);

        UpdateClient setUp = new UpdateClient { FirstName = "John", LastName = "Doe", DOB = new DateOnly(2003, 12, 22)};
        
        //Act
        var result = await SUT.Update(createdClientId, setUp);
        
        //Assert        
        Assert.That(result, "Client is not updated");
        var updatedClient = await SUT.GetBy(createdClientId);
        Assert.That(updatedClient.LastName == "Doe", "client last name is not updated as expected");
        Assert.That(updatedClient.DOB == new DateOnly(2003,12,22) , "client DOB is not updated as expected");
    }

    [Test]
    public async Task Verify_PatchClient()
    {
        //Arrange
       CreateClient client1 = new CreateClient { FirstName = "Kevin", LastName = "Smith", DOB = new DateOnly(1998, 08, 12)};
        var createdClientId = await SUT.Create(client1);

        UpdateClient setUp = new UpdateClient { FirstName = "Jim"};
       
        //Act
        var result  = await SUT.Patch(createdClientId, setUp);

        //Assert        
        Assert.That(result, "Client is not updated");
        var updatedClient = await SUT.GetBy(createdClientId);
        Assert.That(updatedClient.FirstName == "Jim", "client first name is not updated as expected");
        Assert.That(updatedClient.LastName == "Smith", "client last name is not updated as expected");
        Assert.That(updatedClient.DOB == new DateOnly(1998,08,12),  "client DOB is not updated as expected");
    }

    [Test]
    public async Task Verify_DeleteClient()
    {
        //Arrange
        CreateClient client = new CreateClient { FirstName = "John", LastName = "Smith", DOB = new DateOnly(2001, 12, 22)};
        var createdClientId = await SUT.Create(client);

        //Act
        var result = await SUT.Delete(createdClientId);

        //Assert
        Assert.That(result, "Client is not deleted");
        var createdClient = await SUT.GetBy(createdClientId);
        Assert.IsNull(createdClient);

    }
}
