namespace blogest.application.DTOs.responses.Posts;

public record UpdatePostResponse(bool success,string? Title,string? Content,DateTime? LastUpdate,string Message);