using Xunit;
using Moq;
using blogest.infrastructure.Repositories;
using blogest.infrastructure.persistence;
using blogest.application.Interfaces.repositories.Posts;
using AutoMapper;

public class PostsQueryRepositoryTests
{
    [Fact]
    public void Constructor_Should_Instantiate()
    {
        var commentsRepoMock = new Mock<ICommentsQueryRepository>();
        var contextMock = new Mock<BlogCommandContext>(null);
        var mapperMock = new Mock<IMapper>();
        var repo = new PostsQueryRepository(commentsRepoMock.Object, contextMock.Object, mapperMock.Object);
        Assert.NotNull(repo);
    }
}
