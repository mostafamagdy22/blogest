namespace blogest.application.DTOs.responses;

public record CreateCommentResponse(Guid? CommentId,Guid? PostId,Guid? UserId,string Message,bool IsSuccess);