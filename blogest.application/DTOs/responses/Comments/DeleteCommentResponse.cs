namespace blogest.application.DTOs.responses.Comments;

public record DeleteCommentResponse(string Message,Guid CommentId,bool IsSuccess);