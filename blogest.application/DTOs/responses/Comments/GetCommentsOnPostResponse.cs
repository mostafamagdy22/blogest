namespace blogest.application.DTOs.responses.Comments;

public record GetCommentsOnPostResponse(
    List<CommentDto> Comments,
    int TotalCount,
    int PageNumber,
    int PageSize
);