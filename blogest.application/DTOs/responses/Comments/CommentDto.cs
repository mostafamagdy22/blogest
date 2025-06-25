namespace blogest.application.DTOs.responses.Comments;

public record CommentDto(Guid CommentId, string Content, DateTime PublishedAt, string Author)
{
    public CommentDto() : this(Guid.Empty, string.Empty, DateTime.MinValue, string.Empty)
    {
        
    }
};