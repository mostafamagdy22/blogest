namespace blogest.application.DTOs.responses.Comments;

public record CreateCommentResponse(Guid? CommentId,Guid? PostId,Guid? UserId,string Message,bool IsSuccess);