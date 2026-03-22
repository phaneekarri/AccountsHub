using UserAuthApi;
using UserAuthApi.Services;
using UserAuthApi.Settings;
using UserAuthEntities;
using Microsoft.Extensions.Options;
using Moq;

namespace UserAuthApiTest;

public class OtpServiceTests : ServiceTests<OtpService>
{
    private Mock<IOptions<OtpSettings>> otpOptionsMock;

    protected override OtpService SetSUT()
    {
        otpOptionsMock = new Mock<IOptions<OtpSettings>>();
        otpOptionsMock.Setup(o => o.Value).Returns(new OtpSettings { ExpiresInSecs = 300, OtpCodeMin = 100000, OtpCodeMax = 999999 });
        return new OtpService(LoggerMock.Object, Context, otpOptionsMock.Object);
    }
    
    [Test]
    public async Task Verify_CreateOtp()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var result = await SUT.Create(userId, UserIdentifierType.Email);

        // Assert
        Assert.That(result.Id != Guid.Empty, "OTP ID not set");
        Assert.That(result.UserId == userId, "User ID mismatch");
        Assert.That(result.UserIdentifierType == UserIdentifierType.Email, "Identifier type mismatch");
    }

    [Test]
    public async Task Verify_GetRecentOtp()
    {
        // Arrange
        var userId = Guid.NewGuid();
        await SUT.Create(userId, UserIdentifierType.Email);

        // Act
        var result = await SUT.GetRecentOtp(userId, UserIdentifierType.Email);

        // Assert
        Assert.That(result != null, "OTP not found");
        Assert.That(result.UserId == userId, "User ID mismatch");
    }

    [Test]
    public async Task Verify_RemoveOtp()
    {
        // Arrange
        var userId = Guid.NewGuid();
        await SUT.Create(userId, UserIdentifierType.Email);

        // Act
        await SUT.Remove(userId, UserIdentifierType.Email);

        // Assert
        var result = await SUT.GetRecentOtp(userId, UserIdentifierType.Email);
        Assert.That(result == null, "OTP not removed");
    }
}