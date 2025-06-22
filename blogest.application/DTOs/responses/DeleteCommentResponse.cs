namespace blogest.application.DTOs.responses;

public record DeleteCommentResponse(string Message,Guid CommentId,bool IsSuccess);