using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using UserAuthApi.Services;
using UserAuthEntities;
using Infra.Notification;
using Infra;

public class PasswordResetServiceTests : IDisposable
{
    private AuthDBContext _context;
    private Mock<INotificationSender> _mockSender;
    private Mock<IUserService> _mockUserService;
    private PasswordResetService _service;

    public PasswordResetServiceTests()
    {
        var options = new DbContextOptionsBuilder<AuthDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new AuthDBContext(options);
        _mockSender = new Mock<INotificationSender>();
        _mockUserService = new Mock<IUserService>();
        _service = new PasswordResetService(_context, _mockSender.Object, _mockUserService.Object);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task SendResetTokenAsync_UserExists_SendsToken()
    {
        // Arrange
        var request = new UserAuthApi.Dto.ForgotPasswordRequest("test@example.com", UserIdentifierType.Email);
        var user = new User { Id = Guid.NewGuid() };
        _mockUserService.Setup(s => s.Get(UserIdentifierType.Email, "test@example.com")).ReturnsAsync(user);
        _mockSender.Setup(s => s.send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new NotificationResult(200, "Sent"));

        // Act
        var result = await _service.SendResetTokenAsync(request);

        // Assert
        Assert.True(result);
        Assert.Equal(1, _context.PasswordResetTokens.Count());
    }

    // Add more tests...
}