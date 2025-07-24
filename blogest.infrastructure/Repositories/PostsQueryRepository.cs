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

        var publisher = await _context.Users.Where(u => u.Id == post.UserId).Select(u => new { u.Id, u.UserName }).FirstOrDefaultAsync();

        // Paginate comments: default page 1, size 10
        int commentPageNumber = 1;
        int commentPageSize = 10;
        var pagedComments = (post.Comments ?? new List<Comment>())
            .OrderByDescending(c => c.PublishedAt)
            .Skip((commentPageNumber - 1) * commentPageSize)
            .Take(commentPageSize)
            .ToList();

        var commentUserIds = pagedComments.Select(c => c.UserId).Distinct();
        var authors = await _context.Users
            .Where(u => commentUserIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u.UserName);

        var comments = new List<CommentDto>();
        foreach (var comment in pagedComments)
        {
            authors.TryGetValue(comment.UserId, out string? authorName);
            comments.Add(new CommentDto(comment.CommentId, comment.Content, comment.PublishedAt, authorName ?? ""));
        }

        response.UserId = publisher?.Id ?? Guid.Empty;
        response.Publisher = publisher?.UserName ?? string.Empty;
        response.Comments = comments;
        return response;
    }

    public async Task<GetPostsByCategoryResponse> GetPostsByGategory(int categoryId,
        string? include, int pageNumber = 1, int pageSize = 10)
    {
        var query = _context.Posts
            .Include(p => p.Comments)
            .Include(p => p.PostCategories)
            .Where(p => (p.PostCategories ?? new List<PostCategory>()).Any(pc => pc.CategoryId == categoryId));

        int totalCount = await query.CountAsync();
        List<Post> posts = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (posts.Count == 0)
            return new GetPostsByCategoryResponse("no posts found", false, null, totalCount, pageNumber, pageSize);

        List<GetPostResponse> getPosts = _mapper.Map<List<GetPostResponse>>(posts);

        var userIds = posts.Select(p => p.UserId).Distinct();
        var userNames = await _context.Users.Where(u => userIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u.UserName);

        for (int i = 0; i < posts.Count; i++)
        {
            var post = posts[i];
            string? publisher = userNames.TryGetValue(post.UserId, out var name) ? name : string.Empty;
            getPosts[i].Publisher = publisher!;

            var categoryNames = await _context.Categories
                .Where(c => (c.PostCategories ?? new List<PostCategory>()).Any(pc => pc.PostId == post.PostId))
                .Select(c => c.Title)
                .ToListAsync();

            getPosts[i].CategoryNames = categoryNames;
            if (include != null && include.Contains("comments"))
            {
                // Paginate comments for each post
                GetCommentsOnPostResponse comments = await _commentsQueryRepository.GetCommentsByPostId(post.PostId, 1, 10);
                getPosts[i].Comments = comments.Comments;
            }
        }

        return new GetPostsByCategoryResponse("returned successfully", true, getPosts, totalCount, pageNumber, pageSize);
    }


    public async Task<GetPostsByCategoryResponse> GetPostsByUser(GetPostsByUserIdQuery query)
    {
        var postsQuery = _context.Posts.Where(p => p.UserId == query.userId);
        int totalCount = await postsQuery.CountAsync();
        var pagedPostsQuery = postsQuery
            .Skip((query.pageNumber - 1) * query.pageSize)
            .Take(query.pageSize);

        List<Post> posts = await pagedPostsQuery.ToListAsync();

        if (posts.Count == 0) return new GetPostsByCategoryResponse(Message: "No posts found to this user", IsSuccess: false, Posts: null, totalCount, query.pageNumber, query.pageSize);

        List<Guid> userIds = await pagedPostsQuery.Select(p => p.UserId).Distinct().ToListAsync();
        Dictionary<Guid, string?> userDict = await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u.UserName);

        var postResponseTasks = posts.Select(async post =>
        {
            GetPostResponse dto = _mapper.Map<GetPostResponse>(post);
            dto.Publisher = userDict.GetValueOrDefault(post.UserId) ?? "UnKnown";
            if (query.include != null && query.include.Contains("comments"))
            {
                // Paginate comments for each post
                GetCommentsOnPostResponse comments = await _commentsQueryRepository.GetCommentsByPostId(post.PostId, 1, 10);
                dto.Comments = comments.Comments;
            }

            return dto;
        });

        var postResponses = await Task.WhenAll(postResponseTasks);

        return new GetPostsByCategoryResponse(
            Message: $"Posts of user {query.userId} returned successfully",
            IsSuccess: true,
            Posts: postResponses.ToList(),
            TotalCount: totalCount,
            PageNumber: query.pageNumber,
            PageSize: query.pageSize
        );
    }

    public async Task<GetUserLikesResponse> GetUserLikes(GetUserLikesQuery query)
    {
        var userId = query.UserId;
        var likedPostsQuery = _context.Posts.Where(p => (p.Likes ?? new List<Like>()).Any(l => l.UserId == userId));
        int totalCount = await likedPostsQuery.CountAsync();
        List<GetPostResponse> postsLiked = await GetLikedPostsFromUser(userId, query.include, query.pageNumber, query.pageSize);
        if (postsLiked == null || postsLiked.Count == 0)
            return new GetUserLikesResponse("No posts found!", false, userId, 0, new List<GetPostResponse>(), 0, query.pageNumber, query.pageSize);

        int countLikes = await LikesCountOfUser(userId);

        return new GetUserLikesResponse($"Posts that user {userId} liked returned successfully",
            true, userId, countLikes, postsLiked, totalCount, query.pageNumber, query.pageSize);
    }

    /// <summary>
    /// Gets the publisher username for a given post.
    /// </summary>
    private async Task<string> PublisherOfPost(Guid postId)
    {
        return await _context.Posts
            .Where(p => p.PostId == postId)
            .Join(_context.Users, p => p.UserId, u => u.Id, (p, u) => u.UserName)
            .FirstOrDefaultAsync() ?? string.Empty;
    }

    /// <summary>
    /// Gets the list of posts liked by a user, including comments and publisher info.
    /// </summary>
    private async Task<List<GetPostResponse>> GetLikedPostsFromUser(Guid userId, string? include, int pageNumber = 1, int pageSize = 10)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return new List<GetPostResponse>();

        IQueryable<Post> likedPosts = _context.Posts
            .Where(p => (p.Likes ?? new List<Like>()).Any(l => l.UserId == userId));

        var query = likedPosts
            .Join(_context.Users,
                post => post.UserId,
                user => user.Id,
                (post, user) => new
                {
                    Post = post,
                    User = user
                }
            );

        List<GetPostResponse> results = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new GetPostResponse
            {
                PostId = x.Post.PostId,
                Title = x.Post.Title,
                Content = x.Post.Content,
                PublishAt = x.Post.PublishedAt,
                Publisher = x.User.UserName!,
                Comments = new List<CommentDto>(),
                UserId = x.User.Id
            })
            .ToListAsync();

        if (!string.IsNullOrEmpty(include) && include.Contains("comments"))
        {
            foreach (var result in results)
            {
                // Paginate comments for each liked post
                var comments = await _commentsQueryRepository.GetCommentsByPostId(result.PostId, 1, 10);
                result.Comments = comments.Comments;
            }
        }

        return results;
    }

    /// <summary>
    /// Gets the number of likes for a user asynchronously.
    /// </summary>
    private async Task<int> LikesCountOfUser(Guid userId)
    {
        return await _context.Likes.CountAsync(l => l.UserId == userId);
    }

    public async Task<GetPostsByCategoryResponse> GetSavePostsByUser(Guid userId, string? include, int pageNumber = 1, int pageSize = 10)
    {
        var query = _context.Posts
            .Where(p => p.Saves.Any(s => s.UserId == userId))
            .Include(p => p.Saves);

        int totalCount = await query.CountAsync();
        List<Post> posts = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (posts.Count == 0)
            return new GetPostsByCategoryResponse("no save posts to this user", false, null, totalCount, pageNumber, pageSize);

        List<GetPostResponse> postsDto = _mapper.Map<List<GetPostResponse>>(posts);

        if (!string.IsNullOrEmpty(include) && include.Contains("comments"))
        {
            foreach (var postDto in postsDto)
            {
                var comments = await _commentsQueryRepository.GetCommentsByPostId(postDto.PostId, 1, 10);
                postDto.Comments = comments.Comments;
            }
        }

        return new GetPostsByCategoryResponse("Saves post of user returned successfully", true, postsDto, totalCount, pageNumber, pageSize);
    }
}