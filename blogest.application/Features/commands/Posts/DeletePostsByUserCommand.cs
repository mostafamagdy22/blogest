using MediatR;

namespace blogest.application.Features.commands.Posts;

public record DeletePostsByUserCommand(Guid userId) : IRequest<DeletePostResponse>;