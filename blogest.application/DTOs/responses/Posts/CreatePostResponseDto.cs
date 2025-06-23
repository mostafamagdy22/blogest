namespace blogest.application.DTOs.responses.Posts;

public record CreatePostResponseDto(Guid? postId, bool isCreatedSuccessfully);