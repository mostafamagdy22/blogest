namespace blogest.application.Interfaces.repositories.Likes;

public interface ILikesQueryRepository
{
    public Task<GetPostLikesResponse> GetPostLikes(Guid postId);
}