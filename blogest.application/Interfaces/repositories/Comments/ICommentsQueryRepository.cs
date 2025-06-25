namespace blogest.application.Interfaces.repositories.Comments;

public interface ICommentsQueryRepository
{
    public Task<GetCommentByIdResponse> GetCommentById(Guid commentId);
    public Task<GetCommentsOnPostResponse> GetCommentsByPostId(Guid postId,int pageNumber = 1,int pageSize = 10);
    public Task<GetCommentsOfUserResponse> GetCommentsOfUser(Guid userId,int pageNumber = 1,int pageSize = 10);
}