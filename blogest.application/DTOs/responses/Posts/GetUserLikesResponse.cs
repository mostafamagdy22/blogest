namespace blogest.application.DTOs.responses.Posts;

public record GetUserLikesResponse(
    string Message,
    bool success,
    Guid? userId,
    int LikesCount,
    List<GetPostResponse>? LikedPosts,
    int TotalCount,
    int PageNumber,
    int PageSize
);