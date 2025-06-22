namespace blogest.application.DTOs.responses;

public record UpdateCommentResponse(bool IsSuccess,string Message,string NewContent,Guid CommentId);