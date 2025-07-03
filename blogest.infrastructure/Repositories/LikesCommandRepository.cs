using blogest.application.DTOs.responses.Likes;
using blogest.application.Interfaces.repositories.Likes;
using blogest.infrastructure.persistence;

namespace blogest.infrastructure.Repositories;

public class LikesCommandRepository : ILikesCommandRepository
{
    private readonly BlogCommandContext _blogCommandContext;
    private readonly IUsersRepository _usersRepository;
    public LikesCommandRepository(IUsersRepository usersRepository, BlogCommandContext blogCommandContext)
    {
        _usersRepository = usersRepository;
        _blogCommandContext = blogCommandContext;
    }
    public async Task<AddLikeResponse> AddLike(Guid postId)
    {
        Guid? userId = _usersRepository.GetUserIdFromCookies();
        if (userId == null)
        return new AddLikeResponse($"User not found to like",false);

        Like like = new Like((Guid)userId,postId);
        await _blogCommandContext.Likes.AddAsync(like);
        await _blogCommandContext.SaveChangesAsync();

        return new AddLikeResponse($"like added successfully to post: {postId} from user: {userId}",true);
    }

    public async Task<UnLikeResponse> UnLike(Guid postId)
    {
        Guid? userId = _usersRepository.GetUserIdFromCookies();
        if (userId == null)
            return new UnLikeResponse("user not found", false);

        Like like = await _blogCommandContext.Likes.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
        if (like == null)
            return new UnLikeResponse("Like not found", false);

        _blogCommandContext.Likes.Remove(like);
        await _blogCommandContext.SaveChangesAsync();

        return new UnLikeResponse($"Unlike done successfully on post: {postId} by user: {userId}",true);
    }
}