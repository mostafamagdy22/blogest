using MediatR;

namespace blogest.application.Features.commands.Comments;

public record CreateCommentCommand(string Content,Guid PostId,Guid UserId) : IRequest<CreateCommentResponse>;