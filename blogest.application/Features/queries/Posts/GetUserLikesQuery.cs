using MediatR;

namespace blogest.application.Features.queries.Posts;

public record GetUserLikesQuery(Guid UserId,string? include,int pageNumber = 1,int pageSize =10) : IRequest<GetUserLikesResponse>;