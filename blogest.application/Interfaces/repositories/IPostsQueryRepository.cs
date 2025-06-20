namespace blogest.application.Interfaces.repositories;

public interface IPostsQueryRepository
{
    public Task<bool> ExistsAsync(Guid postId);
}