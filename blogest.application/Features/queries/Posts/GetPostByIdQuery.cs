using MediatR;

namespace blogest.application.Features.queries.Posts;

public record GetPostByIdQuery(Guid postId) : IRequest<GetPostResponse>;