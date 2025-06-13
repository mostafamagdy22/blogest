namespace blogest.application.DTOs.responses
{
    public record CreatePostResponseDto(Guid? postId,bool isCreatedSuccessfully);
}