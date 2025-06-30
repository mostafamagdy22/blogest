using Xunit;
using Moq;
using blogest.application.Interfaces.services;
using blogest.infrastructure.Services;
using blogest.infrastructure.persistence;
public class JwtServiceTests
{
    [Fact]
    public void GenerateRefreshToken_Should_Return_NonEmpty_String()
    {
        var contextMock = new Mock<BlogCommandContext>(null);
        var service = new JwtService(contextMock.Object);
        var token = service.GenerateRefreshToken();
        Assert.False(string.IsNullOrWhiteSpace(token));
    }
}
