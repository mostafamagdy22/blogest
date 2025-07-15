namespace blogest.application.DTOs.responses.Posts;

public class GetPostResponse
{
    public Guid PostId { get; set; }
    public List<CommentDto>? Comments { get; set; } = new();
    public string Content { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTime? PublishAt { get; set; }
    public string Publisher { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public List<string> CategoryNames { get; set; } = new();
}