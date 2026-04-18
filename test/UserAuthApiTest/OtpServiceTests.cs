using UserAuthApi;
using UserAuthApi.Services;
using UserAuthApi.Settings;
using UserAuthEntities;
using Microsoft.Extensions.Options;
using Moq;

namespace UserAuthApiTest;

public class OtpServiceTests : ServiceTests<OtpService>
{
    private Mock<IOptions<OtpSettings>>? otpOptionsMock;

    protected override OtpService SetSUT()
    {
        otpOptionsMock = new Mock<IOptions<OtpSettings>>();
        otpOptionsMock.Setup(o => o.Value).Returns(new OtpSettings { ExpiresInSecs = 300, OtpCodeMin = 100000, OtpCodeMax = 999999 });
        return new OtpService(LoggerMock.Object, Context, otpOptionsMock.Object);
    }
    
    [Fact]
    public async Task Verify_CreateOtp()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var result = await SUT.Create(userId, UserIdentifierType.Email);

        // Assert
        Assert.True(result.Id != Guid.Empty);
        Assert.Equal(userId, result.UserId);
        Assert.Equal(UserIdentifierType.Email, result.UserIdentifierType);
    }

    [Fact]
    public async Task Verify_GetRecentOtp()
    {
        // Arrange
        var userId = Guid.NewGuid();
        await SUT.Create(userId, UserIdentifierType.Email);

        // Act
        var result = await SUT.GetRecentOtp(userId, UserIdentifierType.Email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.UserId);
    }

    [Fact]
    public async Task Verify_RemoveOtp()
    {
        // Arrange
        var userId = Guid.NewGuid();
        await SUT.Create(userId, UserIdentifierType.Email);

        // Act
        await SUT.Remove(userId, UserIdentifierType.Email);

        // Assert
        var result = await SUT.GetRecentOtp(userId, UserIdentifierType.Email);
        Assert.Null(result);
    }
}