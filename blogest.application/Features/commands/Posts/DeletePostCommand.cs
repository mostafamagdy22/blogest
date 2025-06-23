using MediatR;

namespace blogest.application.Features.commands.Posts;

public record DeletePostCommand(Guid postId) : IRequest<DeletePostResponse>;