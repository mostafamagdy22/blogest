using MediatR;

namespace blogest.application.Features.queries.Comments;

public record GetCommentsOfUserQuery(Guid userId) : IRequest<GetCommentsOfUserResponse>;