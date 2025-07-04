using blogest.application.DTOs.responses.Users;

namespace blogest.application.DTOs.responses.Likes;

public record GetPostLikesResponse(
    string Message,
    bool success,
    Guid PostId,
    int LikesCount,
    bool IsLikedByCurrentUser,
    List<UserDtoResponse>? LikedUsers);