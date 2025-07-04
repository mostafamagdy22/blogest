using AutoMapper;
using blogest.application.DTOs.responses.Likes;
using blogest.application.DTOs.responses.Users;
using blogest.application.Interfaces.repositories.Likes;
using blogest.infrastructure.persistence;

namespace blogest.infrastructure.Repositories;

public class LikesQueryRepository : ILikesQueryRepository
{
    private readonly BlogCommandContext _blogCommandContext;
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;
    private readonly IPostsQueryRepository _postsQueryRepository;
    public LikesQueryRepository(IPostsQueryRepository postsQueryRepository, IMapper mapper, IUsersRepository usersRepository, BlogCommandContext blogCommandContext)
    {
        _postsQueryRepository = postsQueryRepository;
        _mapper = mapper;
        _usersRepository = usersRepository;
        _blogCommandContext = blogCommandContext;
    }
    public async Task<GetPostLikesResponse> GetPostLikes(Guid postId)
    {
        Guid? userId = _usersRepository.GetUserIdFromCookies();
        int likesCount = await LikesCountOfPost(postId);
        bool isCurrentUserLikePost = false;
        if (userId != null)
        {
            isCurrentUserLikePost = await IsCurrentUserLikePost(postId, userId.Value);
        }
        List<UserDtoResponse> usersLikePost = await UsersLikePost(postId);

        GetPostLikesResponse response = new GetPostLikesResponse($"Likes of post: {postId} returned successfully", true, postId, likesCount, isCurrentUserLikePost, usersLikePost);
        return response;
    }
    /// <summary>
    /// Returns the number of likes for a given post asynchronously.
    /// </summary>
    private async Task<int> LikesCountOfPost(Guid postId)
    {
        return await _blogCommandContext.Likes.CountAsync(l => l.PostId == postId);
    }
    /// <summary>
    /// Checks if the current user liked the post using an efficient query.
    /// </summary>
    private async Task<bool> IsCurrentUserLikePost(Guid postId, Guid userId)
    {
        return await _blogCommandContext.Likes.AnyAsync(l => l.PostId == postId && l.UserId == userId);
    }
    /// <summary>
    /// Returns a list of users who liked a given post.
    /// </summary>
    private async Task<List<UserDtoResponse>> UsersLikePost(Guid postId)
    {
        List<UserDtoResponse> result = await
         _blogCommandContext.Likes.Where(l => l.PostId == postId).Join(
        _blogCommandContext.Users,
        like => like.UserId,
        user => user.Id,
        (like, user) => new UserDtoResponse(user.Id, user.UserName)).ToListAsync();

        return result;
    }
    
}