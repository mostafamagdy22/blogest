namespace blogest.application.DTOs.responses.Comments;

public record GetCommentsOfUserResponse(
    List<CommentDto> Comments,
    int TotalCount,
    int PageNumber,
    int PageSize
);