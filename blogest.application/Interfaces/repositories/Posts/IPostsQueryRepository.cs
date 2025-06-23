namespace blogest.application.Interfaces.repositories.Posts;

public interface IPostsQueryRepository
{
    public Task<bool> ExistsAsync(Guid postId);
    public Task<GetPostResponse> GetPostByIdAsync(Guid postId);
}