namespace blogest.application.Interfaces.repositories.Comments;

public interface ICommentsCommandRepository
{
    /// <summary>
    /// Creates a new comment.
    /// </summary>
    /// <param name="comment">The comment entity to create.</param>
    /// <returns>Response containing the created comment details.</returns>
    public Task<CreateCommentResponse> CreateComment(Comment comment);

    /// <summary>
    /// Deletes a comment by its ID.
    /// </summary>
    /// <param name="commentId">The comment ID to delete.</param>
    /// <returns>Response indicating the result of the delete operation.</returns>
    public Task<DeleteCommentResponse> DeleteComment(Guid commentId);

    /// <summary>
    /// Updates a comment's content by its ID.
    /// </summary>
    /// <param name="commentId">The comment ID to update.</param>
    /// <param name="newContent">The new content for the comment.</param>
    /// <returns>Response indicating the result of the update operation.</returns>
    public Task<UpdateCommentResponse> UpdateComment(Guid commentId,string newContent);
}