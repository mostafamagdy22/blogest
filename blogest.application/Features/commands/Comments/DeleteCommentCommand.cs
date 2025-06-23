using MediatR;

namespace blogest.application.Features.commands.Comments;

public record DeleteCommentCommand(Guid CommentId) : IRequest<DeleteCommentResponse>;