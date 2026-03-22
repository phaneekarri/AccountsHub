using UserAuthApi;
using UserAuthApi.Services;
using UserAuthApi.Settings;
using UserAuthEntities;
using Microsoft.Extensions.Options;
using Moq;

namespace UserAuthApiTest;

public class JwtServiceTests
{
    private JwtService sut;
    private Mock<IOptions<JwtSettings>> jwtOptionsMock;

    [SetUp]
    public void SetUp()
    {
        jwtOptionsMock = new Mock<IOptions<JwtSettings>>();
        jwtOptionsMock.Setup(o => o.Value).Returns(new JwtSettings { Secret = "supersecretkeythatislongenoughforhmacsha256", Issuer = "test", Audience = "test" });
        sut = new JwtService(jwtOptionsMock.Object);
    }
    
    [Test]
    public void Verify_GenerateToken()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid(), Email = "test@example.com" };

        // Act
        var result = sut.GenerateToken(user);

        // Assert
        Assert.That(!string.IsNullOrEmpty(result.accessToken), "Token not generated");
        Assert.That(result.expiresInSecs > 0, "Expiry not set");
    }
}