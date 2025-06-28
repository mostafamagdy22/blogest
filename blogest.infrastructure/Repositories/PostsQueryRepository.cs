using AutoMapper;
using blogest.infrastructure.persistence;

namespace blogest.infrastructure.Repositories;

public class PostsQueryRepository : IPostsQueryRepository
{
    private readonly BlogCommandContext _context;
    private readonly IMapper _mapper;
    private readonly ICommentsQueryRepository _commentsQueryRepository;
    public PostsQueryRepository(ICommentsQueryRepository commentsQueryRepository, BlogCommandContext context, IMapper mapper)
    {
        _commentsQueryRepository = commentsQueryRepository;
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

        foreach (var comment in post.Comments)
        {
            var author = await _context.Users.Where(u => u.Id == comment.UserId).Select(u => u.UserName).FirstOrDefaultAsync();
            comments.Add(new CommentDto(comment.CommentId, comment.Content, comment.PublishedAt, author));
        }


        return response with { Publisher = publisher, Comments = comments };
    }

    public async Task<GetPostsByCategoryResponse> GetPostsByGategory(int categoryId,
        string? include, int pageNumber = 1, int pageSize = 10)
    {
        List<Post> posts = await _context.Posts
            .Include(p => p.Comments)
            .Include(p => p.PostCategories)
            .Where(p => p.PostCategories.Any(pc => pc.CategoryId == categoryId))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (posts.Count == 0)
            return new GetPostsByCategoryResponse("no posts found", false, null);

        List<GetPostResponse> getPosts = _mapper.Map<List<GetPostResponse>>(posts);

        for (int i = 0; i < posts.Count; i++)
        {
            var post = posts[i];
            string publisher = await _context.Users
                .Where(u => u.Id == post.UserId)
                .Select(u => u.UserName)
                .FirstOrDefaultAsync() ?? string.Empty;
            if (include.Contains("comments"))
            {
                GetCommentsOnPostResponse comments = await _commentsQueryRepository.GetCommentsByPostId(post.PostId, pageNumber, pageSize);
                List<CommentDto> dtos = comments.Comments;
                var mapped = getPosts[i];
                getPosts[i] = mapped with { Publisher = publisher, Comments = dtos };

            }
        }

        return new GetPostsByCategoryResponse("returned successfully", true, getPosts);
    }

    public async Task<GetPostsByCategoryResponse> GetPostsByUser(GetPostsByUserIdQuery query)
    {
        IQueryable<Post> postsQuery = _context.Posts.Where(p => p.UserId == query.userId)
                        .Skip((query.pageNumber - 1) * query.pageSize)
                        .Take(query.pageSize);

        List<Post> posts = await postsQuery.ToListAsync();

        if (posts.Count == 0) return new GetPostsByCategoryResponse(Message: "No posts found to this user",IsSuccess:false,Posts: null);

        List<Guid> userIds = await postsQuery.Select(p => p.UserId).Distinct().ToListAsync();
        Dictionary<Guid, string?> userDict = await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u.UserName);

        var postResponseTasks = posts.Select(async post =>
        {
            GetPostResponse dto = _mapper.Map<GetPostResponse>(post);
            dto = dto with { Publisher = userDict.GetValueOrDefault(post.UserId) ?? "UnKnown" };
            if (query.include == "comments")
            {
                GetCommentsOnPostResponse comments = await _commentsQueryRepository.GetCommentsByPostId(post.PostId);
                dto = dto with { Comments = comments.Comments };
            }

            return dto;
        });

        var postResponses = await Task.WhenAll(postResponseTasks);

        return new GetPostsByCategoryResponse(
            Message: $"Posts of user {query.userId} returned successfully",
            IsSuccess: true,
            Posts: postResponses.ToList()
        );
    }
}