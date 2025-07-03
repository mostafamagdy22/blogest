namespace blogest.application.DTOs.responses.Likes
{
    /// <summary>
    /// Response for removing a like from a post.
    /// </summary>
    public record UnLikeResponse(string message,bool success);
}
