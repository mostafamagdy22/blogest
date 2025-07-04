using MediatR;

namespace blogest.application.Features.queries.Likes;

public record GetPostLikesQuery(Guid PostId) : IRequest<GetPostLikesResponse>;