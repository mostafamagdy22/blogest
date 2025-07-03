using MediatR;

namespace blogest.application.Features.commands.Likes
{
    /// <summary>
    /// Command to remove a like from a post.
    /// </summary>
    public record UnLikeCommand(Guid postId) : IRequest<UnLikeResponse>;
}
