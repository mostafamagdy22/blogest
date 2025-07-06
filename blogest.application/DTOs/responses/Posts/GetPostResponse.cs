namespace blogest.application.DTOs.responses.Posts;

public record GetPostResponse
{
    public Guid PostId { get; init; }
    public List<CommentDto>? Comments { get; set; } = new();
    public string Content { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public DateTime? PublishAt { get; init; }
    public string Publisher { get; init; } = string.Empty;
    public Guid UserId { get; init; }
}
