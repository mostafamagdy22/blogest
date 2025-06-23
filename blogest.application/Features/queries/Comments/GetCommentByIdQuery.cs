using MediatR;

namespace blogest.application.Features.queries.Comments;

public record GetCommentByIdQuery(Guid CommentId) : IRequest<GetCommentByIdResponse>;