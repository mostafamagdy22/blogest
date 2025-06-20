using blogest.application.Interfaces.repositories;
using blogest.infrastructure.persistence;

namespace blogest.infrastructure.Repositories;

public class PostsQueryRepository : IPostsQueryRepository
{
    private readonly BlogCommandContext _context;
    public PostsQueryRepository(BlogCommandContext context)
    {
        _context = context;
    }
    public async Task<bool> ExistsAsync(Guid postId)
    {
        return await _context.Posts.FindAsync(postId) != null;
    }
}