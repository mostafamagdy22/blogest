namespace blogest.application.DTOs.responses.Comments;

public record CommentDto(Guid CommentId, string Content, DateTime PublishedAt, string Author);