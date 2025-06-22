using blogest.application.DTOs.responses;

namespace blogest.application.Interfaces.repositories;

public interface IPostsQueryRepository
{
    public Task<bool> ExistsAsync(Guid postId);
    public Task<GetPostResponse> GetPostByIdAsync(Guid postId);
}