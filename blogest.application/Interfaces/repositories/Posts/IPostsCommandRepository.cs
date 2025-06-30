namespace blogest.application.Interfaces.repositories.Posts;

public interface IPostsCommandRepository
{
    /// <summary>
    /// Adds a new post asynchronously.
    /// </summary>
    /// <param name="post">The post entity to add.</param>
    /// <returns>The ID of the created post.</returns>
    public Task<Guid> AddAsync(Post post,List<int> categoryIds);
    public Task<UpdatePostCategoriesResponse> updatePostCategories(UpdatePostCategoriesCommand command);
    /// <summary>
    /// Deletes a post by its ID.
    /// </summary>
    /// <param name="postId">The post ID to delete.</param>
    /// <returns>Response indicating the result of the delete operation.</returns>
    public Task<DeletePostResponse> DeletePost(Guid postId);

    /// <summary>
    /// Updates a post using the provided command.
    /// </summary>
    /// <param name="updatePostCommand">The update command containing new post data.</param>
    /// <returns>Response indicating the result of the update operation.</returns>
    public Task<UpdatePostResponse> UpdatePost(UpdatePostCommand updatePostCommand);

    /// <summary>
    /// Deletes all posts for a specific user.
    /// </summary>
    /// <param name="userId">The user ID whose posts will be deleted.</param>
    /// <returns>Response indicating the result of the delete operation.</returns>
    public Task<DeletePostResponse> DeletePostsByUser(Guid userId);
}
