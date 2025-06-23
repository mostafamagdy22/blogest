using MediatR;

namespace blogest.application.Features.commands.Comments;

public record UpdateCommentCommand(Guid CommentId,string NewContent) : IRequest<UpdateCommentResponse>;