namespace blogest.domain.Entities;

public class Save
{
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }
    public Post Post { get; set; }
}