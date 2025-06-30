namespace blogest.application.DTOs.responses.Posts;

public record CreatePostResponseDto(string Message,Guid? postId, bool isCreatedSuccessfully);