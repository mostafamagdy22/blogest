using blogest.application.DTOs.responses;

namespace blogest.application.Interfaces.repositories;

public interface ICommentsQueryRepository
{
    public Task<GetCommentByIdResponse> GetCommentById(Guid commentId);
}