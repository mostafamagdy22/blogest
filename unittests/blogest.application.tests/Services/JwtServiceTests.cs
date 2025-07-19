using Xunit;
using Moq;
using blogest.application.Interfaces.services;
using blogest.infrastructure.Services;
using blogest.infrastructure.persistence;
using Microsoft.EntityFrameworkCore;

namespace blogest.application.tests.Services
{
    public class JwtServiceTests
    {
        [Fact]
        public void GenerateRefreshToken_Should_Return_NonEmpty_String()
        {
            var options = new DbContextOptionsBuilder<BlogCommandContext>()
    .UseInMemoryDatabase("JwtServiceTestDb")
    .Options;
            var context = new BlogCommandContext(options);
            var service = new JwtService(context);
            var token = service.GenerateRefreshToken();

            Assert.False(string.IsNullOrWhiteSpace(token));
        }
    }
}
