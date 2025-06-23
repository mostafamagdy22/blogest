using blogest.application.DTOs.responses.Comments;

namespace blogest.application.DTOs.responses.Posts;

public record GetPostResponse(List<CommentDto> Comments, string Content, string Title, DateTime PublishAt, string Publisher);
