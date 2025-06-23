namespace blogest.application.DTOs.responses.Posts;

public record UpdatePostResponse(string? Title,string? Content,DateTime? LastUpdate,string Message);