namespace blogest.application.Interfaces.repositories.Posts;

public interface IPostsQueryRepository
{
    /// <summary>
    /// Checks if a post with the given ID exists.
    /// </summary>
    /// <param name="postId">The post ID to check.</param>
    /// <returns>True if the post exists; otherwise, false.</returns>
    public Task<bool> ExistsAsync(Guid postId);

    /// <summary>
    /// Gets a post by its ID.
    /// </summary>
    /// <param name="postId">The post ID.</param>
    /// <returns>The post response if found; otherwise, null.</returns>
    public Task<GetPostResponse> GetPostByIdAsync(Guid postId);

    /// <summary>
    /// Gets posts by category with optional includes and pagination.
    /// </summary>
    /// <param name="categoryId">The category ID.</param>
    /// <param name="include">Related entities to include (e.g., comments).</param>
    /// <param name="pageNumber">Page number for pagination.</param>
    /// <param name="pageSize">Page size for pagination.</param>
    /// <returns>Paginated posts response for the category.</returns>
    public Task<GetPostsByCategoryResponse> GetPostsByGategory(int categoryId, string? include, int pageNumber = 1, int pageSize = 10);

    /// <summary>
    /// Gets posts by user with optional includes and pagination.
    /// </summary>
    /// <param name="query">Query object containing userId, include, pageNumber, and pageSize.</param>
    /// <returns>Paginated posts response for the user.</returns>
    public Task<GetPostsByCategoryResponse> GetPostsByUser(GetPostsByUserIdQuery query);
    public Task<GetUserLikesResponse> GetUserLikes(GetUserLikesQuery query);
}