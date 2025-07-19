using Xunit;
using Moq;
using blogest.application.Validation.Posts;
using blogest.application.Interfaces.repositories.Posts;
using blogest.application.Features.commands.Posts;

namespace blogest.application.tests.Validators
{
    public class UpdatePostCommandValidatorTests
    {
        [Fact]
        public async Task Should_Fail_When_Title_Is_Too_Short()
        {
            var repoMock = new Mock<IPostsQueryRepository>();
            repoMock.Setup(r => r.ExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            var validator = new blogest.application.Validation.UpdatePostCommandValidator(repoMock.Object);
            var command = new blogest.application.Features.commands.Posts.UpdatePostCommand("a", "valid content for post", Guid.NewGuid());
            var result = await validator.ValidateAsync(command);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Title");
        }

        [Fact]
        public async Task Should_Fail_When_Post_Does_Not_Exist()
        {
            var repoMock = new Mock<IPostsQueryRepository>();
            repoMock.Setup(r => r.ExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            var validator = new blogest.application.Validation.UpdatePostCommandValidator(repoMock.Object);
            var command = new blogest.application.Features.commands.Posts.UpdatePostCommand("Valid Title", "valid content for post", Guid.NewGuid());
            var result = await validator.ValidateAsync(command);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "postId");
        }
    }
}

