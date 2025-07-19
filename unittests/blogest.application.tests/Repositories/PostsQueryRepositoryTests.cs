using Xunit;
using Moq;
using blogest.infrastructure.Repositories;
using blogest.infrastructure.persistence;
using blogest.application.Interfaces.repositories.Comments;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace blogest.application.tests.Repositories
{
    public class PostsQueryRepositoryTests
    {
        [Fact]
        public void Constructor_Should_Instantiate()
        {
            var options = new DbContextOptionsBuilder<BlogCommandContext>()
                .UseInMemoryDatabase("TestDb_PostsQueryRepo")
                .Options;

            var context = new BlogCommandContext(options);

            var commentsRepoMock = new Mock<ICommentsQueryRepository>();
            var mapperMock = new Mock<IMapper>();

            var repo = new PostsQueryRepository(commentsRepoMock.Object, context, mapperMock.Object);

            Assert.NotNull(repo);
        }
    }
}
