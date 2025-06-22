using AutoMapper;
using blogest.application.DTOs.responses;
using blogest.application.Interfaces.repositories;
using blogest.infrastructure.persistence;

namespace blogest.infrastructure.Repositories;

public class PostsQueryRepository : IPostsQueryRepository
{
    private readonly BlogCommandContext _context;
    private readonly IMapper _mapper;
    public PostsQueryRepository(BlogCommandContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }
    public async Task<bool> ExistsAsync(Guid postId)
    {
        return await _context.Posts.FindAsync(postId) != null;
    }

    public async Task<GetPostResponse> GetPostByIdAsync(Guid postId)
    {
        var post = await _context.Posts
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.PostId == postId);

        if (post is null)
            return null;

        var response = _mapper.Map<GetPostResponse>(post);

        var publisher = await _context.Users.Where(u => u.Id == post.UserId).Select(u => u.UserName).FirstOrDefaultAsync();

        var comments = new List<CommentDto>();

        foreach(var comment in post.Comments)
        {
            var author = await _context.Users.Where(u => u.Id == comment.UserId).Select(u => u.UserName).FirstOrDefaultAsync();
            comments.Add(new CommentDto(comment.CommentId, comment.Content, comment.PublishedAt, author));
        }


        return response with { Publisher = publisher, Comments = comments };
    }
}