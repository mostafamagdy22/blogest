namespace blogest.application.DTOs.responses.Comments;

public record UpdateCommentResponse(bool IsSuccess, string Message, string? NewContent, Guid CommentId);