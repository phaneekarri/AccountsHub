using UserAuthApi;
using UserAuthApi.Services;
using UserAuthEntities;
using UserAuthEntities.InternalUsers;

namespace UserAuthApiTest;

public class UserServiceTests : ServiceTests<UserService>
{
    protected override UserService SetSUT() => new UserService(LoggerMock.Object, Context);
    
    [Fact]
    public async Task Verify_CreateUser()
    {
        // Arrange
        var user = new User { Email = "test@example.com", Phone = "1234567890" };

        // Act
        var result = await SUT.Create(user);

        // Assert
        Assert.True(result.Id != Guid.Empty);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task Verify_GetUserById()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };
        var created = await SUT.Create(user);

        // Act
        var result = await SUT.Get(created.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task Verify_GetUserByEmail()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };
        await SUT.Create(user);

        // Act
        var result = await SUT.Get(UserIdentifierType.Email, "test@example.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task Verify_GetInternalUser()
    {
        // Arrange
        var user = new User { Email = "test@example.com", UserName = "testuser" };
        await SUT.Create(user, "password123");

        // Act
        var result = await SUT.Get("testuser");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("testuser", result.User.UserName);
    }

    [Fact]
    public async Task Verify_EnableMFA()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };
        var internalUser = await SUT.Create(user, "password123");

        // Act
        var result = await SUT.EnableMFA(internalUser.UserId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsMFAEnabled);
    }
}