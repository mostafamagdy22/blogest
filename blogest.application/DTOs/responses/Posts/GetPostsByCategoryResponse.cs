namespace blogest.application.DTOs.responses.Posts;

public record GetPostsByCategoryResponse(
    string Message,
    bool IsSuccess,
    List<GetPostResponse>? Posts,
    int TotalCount,
    int PageNumber,
    int PageSize
);