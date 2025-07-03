namespace blogest.domain.Entities;

public class Like
{
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }
    public Post Post { get; set; }
    public Like(Guid userId, Guid postId)
    {
        UserId = userId;
        PostId = postId;
    }
}