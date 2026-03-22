using UserAuthApi;
using UserAuthApi.Services;
using UserAuthApi.Dto;
using UserAuthApi.Settings;
using UserAuthEntities;
using Microsoft.Extensions.Options;
using Moq;

namespace UserAuthApiTest;

public class TokenServiceTests : ServiceTests<TokenService>
{
    private Mock<JwtService> jwtServiceMock;

    protected override TokenService SetSUT()
    {
        var jwtOptionsMock = new Mock<IOptions<JwtSettings>>();
        jwtOptionsMock.Setup(o => o.Value).Returns(new JwtSettings { Secret = "supersecretkeythatislongenoughforhmacsha256", Issuer = "test", Audience = "test" });
        jwtServiceMock = new Mock<JwtService>(jwtOptionsMock.Object);
        jwtServiceMock.Setup(j => j.GenerateToken(It.IsAny<User>())).Returns(new AuthTokenModel("mockToken", 3600));
        return new TokenService(jwtServiceMock.Object, LoggerMock.Object, Context);
    }
    
    [Test]
    public void Verify_GenerateToken()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid(), Email = "test@example.com" };

        // Act
        var result = SUT.GenerateToken(user);

        // Assert
        Assert.That(result.accessToken == "mockToken", "Token not generated");
        Assert.That(result.expiresInSecs == 3600, "Expiry mismatch");
    }
}