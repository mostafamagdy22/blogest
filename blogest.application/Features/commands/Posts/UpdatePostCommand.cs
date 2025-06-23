using MediatR;

namespace blogest.application.Features.commands.Posts;

public record UpdatePostCommand(string? Title,string? Content,Guid postId) : IRequest<UpdatePostResponse>;