namespace blogest.application.DTOs.responses;

public record CommentDto(Guid CommentId, string Content, DateTime PublishedAt, string Author);