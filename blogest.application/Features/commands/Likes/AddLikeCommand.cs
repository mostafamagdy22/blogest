using MediatR;

namespace blogest.application.Features.commands.Likes
{
    /// <summary>
    /// Command to add a like to a post.
    /// </summary>
    public record AddLikeCommand(Guid postId) : IRequest<AddLikeResponse>;
}
