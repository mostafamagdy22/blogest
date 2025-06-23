namespace blogest.application.DTOs.responses;

public record GetCommentByIdResponse(string content,string userName,Guid postId);