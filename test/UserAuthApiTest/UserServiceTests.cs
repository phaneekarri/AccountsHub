using UserAuthApi;
using UserAuthApi.Services;
using UserAuthEntities;

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
        Assert.Equal("testuser", result.UserName);
    }

    [Fact]
    public async Task Verify_EnableMFA()
    {
        // Arrange
        var user = new User { Email = "test@example.com", UserName = "testuser" };
        var internalUser = await SUT.Create(user, "password123");

        // Act
        var result = await SUT.EnableMFA(internalUser.Id, MfaMethod.EmailOtp);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.MfaEnabled);
    }

    [Fact]
    public async Task Verify_VerifyPassword_CorrectPassword()
    {
        // Arrange
        var user = new User { Email = "test@example.com", UserName = "testuser" };
        await SUT.Create(user, "password123");

        // Act
        var result = await SUT.VerifyPassword("testuser", "password123");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Verify_VerifyPassword_IncorrectPassword()
    {
        // Arrange
        var user = new User { Email = "test@example.com", UserName = "testuser" };
        await SUT.Create(user, "password123");

        // Act
        var result = await SUT.VerifyPassword("testuser", "wrongpassword");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Verify_VerifyPassword_NonExistentUser()
    {
        // Act
        var result = await SUT.VerifyPassword("nonexistent", "password123");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Verify_CreateOrLinkOAuthUser_NewUser()
    {
        // Act
        var result = await SUT.CreateOrLinkOAuthUser("newuser@example.com", OAuthProvider.Google, "google123", "newuser@example.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("newuser@example.com", result.Email);
        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public async Task Verify_CreateOrLinkOAuthUser_LinkToExistingUser()
    {
        // Arrange
        var existingUser = new User { Email = "existing@example.com" };
        await SUT.Create(existingUser);

        // Act
        var result = await SUT.CreateOrLinkOAuthUser("existing@example.com", OAuthProvider.Google, "google123", "existing@example.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("existing@example.com", result.Email);
        Assert.Equal(existingUser.Id, result.Id);
    }
}