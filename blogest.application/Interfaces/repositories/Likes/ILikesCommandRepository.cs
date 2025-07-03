namespace blogest.application.Interfaces.repositories.Likes;

public interface ILikesCommandRepository
{
    public Task<AddLikeResponse> AddLike(Guid postId);
    public Task<UnLikeResponse> UnLike(Guid postId);
}