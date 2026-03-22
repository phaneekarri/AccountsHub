using UserAuthApi;
using UserAuthApi.Services;
using UserAuthEntities;
using UserAuthEntities.InternalUsers;

namespace UserAuthApiTest;

public class UserServiceTests : ServiceTests<UserService>
{
    protected override UserService SetSUT() => new UserService(LoggerMock.Object, Context);
    
    [Test]
    public async Task Verify_CreateUser()
    {
        // Arrange
        var user = new User { Email = "test@example.com", Phone = "1234567890" };

        // Act
        var result = await SUT.Create(user);

        // Assert
        Assert.That(result.Id != Guid.Empty, "User ID not set");
        Assert.That(result.Email == "test@example.com", "Email not set");
    }

    [Test]
    public async Task Verify_GetUserById()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };
        var created = await SUT.Create(user);

        // Act
        var result = await SUT.Get(created.Id);

        // Assert
        Assert.That(result != null, "User not found");
        Assert.That(result.Email == "test@example.com", "Email mismatch");
    }

    [Test]
    public async Task Verify_GetUserByEmail()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };
        await SUT.Create(user);

        // Act
        var result = await SUT.Get(UserIdentifierType.Email, "test@example.com");

        // Assert
        Assert.That(result != null, "User not found");
        Assert.That(result.Email == "test@example.com", "Email mismatch");
    }

    [Test]
    public async Task Verify_GetInternalUser()
    {
        // Arrange
        var user = new User { Email = "test@example.com", UserName = "testuser" };
        await SUT.Create(user, "password123");

        // Act
        var result = await SUT.Get("testuser");

        // Assert
        Assert.That(result != null, "Internal user not found");
        Assert.That(result.User.UserName == "testuser", "Username mismatch");
    }

    [Test]
    public async Task Verify_EnableMFA()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };
        var internalUser = await SUT.Create(user, "password123");

        // Act
        var result = await SUT.EnableMFA(internalUser.UserId);

        // Assert
        Assert.That(result != null, "MFA not enabled");
        Assert.That(result.IsMFAEnabled, "MFA not enabled");
    }
}