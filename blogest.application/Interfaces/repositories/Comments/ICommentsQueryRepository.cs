namespace blogest.application.Interfaces.repositories.Comments;

public interface ICommentsQueryRepository
{
    public Task<GetCommentByIdResponse> GetCommentById(Guid commentId);
}