using MediatR;

namespace blogest.application.Features.queries.Comments;

public record GetCommentsByPostIdQuery(Guid postId,int pageNumber = 1,int pageSize = 10) : IRequest<GetCommentsOnPostResponse>;