namespace blogest.application.DTOs.responses;

public record UpdatePostResponse(string? Title,string? Content,DateTime? LastUpdate,string Message);