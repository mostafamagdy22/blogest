namespace blogest.application.DTOs.responses.Users;

public class GetUserInfoResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public string? UserName { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? Email { get; set; }
    public List<GetPostResponse>? PostsOfUser { get; set; }
    public Guid userId { get; set; }
}