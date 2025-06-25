namespace blogest.application.Interfaces.repositories.Posts;

public interface IPostsQueryRepository
{
    public Task<bool> ExistsAsync(Guid postId);
    public Task<GetPostResponse> GetPostByIdAsync(Guid postId);
    public Task<GetPostsByCategoryResponse> GetPostsByGategory(int categoryId,string? include,int pageNumber = 1,int pageSize = 10);
}