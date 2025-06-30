using Xunit;
using Moq;
using blogest.application.Validation;
using blogest.application.Interfaces.repositories.Posts;
using blogest.application.Features.commands.Posts;

public class UpdatePostCommandValidatorTests
{
    [Fact]
    public async Task Should_Fail_When_Title_Is_Too_Short()
    {
        var repoMock = new Mock<IPostsQueryRepository>();
        repoMock.Setup(r => r.ExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);
        var validator = new UpdatePostCommandValidator(repoMock.Object);
        var command = new UpdatePostCommand { Title = "a", Content = "valid content for post", postId = Guid.NewGuid() };
        var result = await validator.ValidateAsync(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Title");
    }

    [Fact]
    public async Task Should_Fail_When_Post_Does_Not_Exist()
    {
        var repoMock = new Mock<IPostsQueryRepository>();
        repoMock.Setup(r => r.ExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);
        var validator = new UpdatePostCommandValidator(repoMock.Object);
        var command = new UpdatePostCommand { Title = "Valid Title", Content = "valid content for post", postId = Guid.NewGuid() };
        var result = await validator.ValidateAsync(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "postId");
    }
}
