using AutoMapper;
using blogest.infrastructure.Identity;
using blogest.infrastructure.persistence;

namespace blogest.infrastructure.Repositories;

public class CommentsQueryRepository : ICommentsQueryRepository
{
    private readonly BlogCommandContext _context;
    private readonly IMapper _mapper;
    public CommentsQueryRepository(BlogCommandContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<GetCommentByIdResponse> GetCommentById(Guid commentId)
    {
        var comment1 = await _context.Comments.Join(_context.Users,
        comment => comment.UserId,
        user => user.Id,
        (comment, user) => new { Comment = comment, User = user }).ToListAsync();
        var comment = comment1.FirstOrDefault(c => c.Comment.CommentId == commentId);
        if (comment == null)
            throw new Exception("No comment by this id!");


        return new GetCommentByIdResponse(
            content: comment.Comment.Content,
            userName: comment.User.UserName,
            postId: comment.Comment.PostId
        );
    }

    public async Task<GetCommentsOnPostResponse> GetCommentsByPostId(Guid postId,int pageNumber = 1,int pageSize = 10)
    {
           Post post = await _context.Posts
        .Include(p => p.Comments)
        .FirstOrDefaultAsync(p => p.PostId == postId);

    if (post == null)
        throw new Exception("No post by this id!");

    if (post.Comments == null || post.Comments.Count == 0)
        return new GetCommentsOnPostResponse(new List<CommentDto>());

    List<Guid> userIds = post.Comments.Select(c => c.UserId).Distinct().ToList();
    List<AppUser> users = await _context.Users
        .Where(u => userIds.Contains(u.Id))
        .ToListAsync();

    
    var userDict = users.ToDictionary(u => u.Id, u => u.UserName);

    List<CommentDto> comments = post.Comments.Select(c =>
    {
        userDict.TryGetValue(c.UserId, out var userName);
        userName ??= "Unknown User";  
        
        return new CommentDto(c.CommentId, c.Content, c.PublishedAt, userName);
    }).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

    return new GetCommentsOnPostResponse(comments);
    }

    public async Task<GetCommentsOfUserResponse> GetCommentsOfUser(Guid userId,int pageNumber = 1,int pageSize = 10)
    {
        AppUser user = await _context.Users.FindAsync(userId);

        if (user == null)
            throw new Exception("No user by this id!");

        List<Comment> query = await _context.Comments.Where(c => c.UserId == userId).ToListAsync();
        if (query == null || query.Count == 0)
            return new GetCommentsOfUserResponse(new List<CommentDto>());

        List<CommentDto> comments = query.Select(c => _mapper.Map<CommentDto>(c)).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new GetCommentsOfUserResponse(comments);
    }
}