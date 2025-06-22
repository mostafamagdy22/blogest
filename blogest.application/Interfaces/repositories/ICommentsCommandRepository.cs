using blogest.application.DTOs.responses;
using blogest.domain.Entities;

namespace blogest.application.Interfaces.repositories;

public interface ICommentsCommandRepository
{
    public Task<CreateCommentResponse> CreateComment(Comment comment);
    public Task<DeleteCommentResponse> DeleteComment(Guid commentId);
    public Task<UpdateCommentResponse> UpdateComment(Guid commentId,string newContent);
}