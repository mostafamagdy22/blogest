namespace blogest.application.DTOs.responses.Comments;

public record GetCommentByIdResponse(string content,string userName,Guid postId);