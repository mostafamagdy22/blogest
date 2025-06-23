using MediatR;

namespace blogest.application.Features.commands.Posts;

public class CreatePostCommand : IRequest<CreatePostResponseDto>
{
    public string Title { get; init; }
    public string Content { get; init; }
    public DateTime PublishDate { get; init; }
}