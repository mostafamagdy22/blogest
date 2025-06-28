using MediatR;

namespace blogest.application.Features.queries.Posts;

public record GetPostsByUserIdQuery(Guid userId,string? include,int pageNumber = 1,int pageSize = 10) : IRequest<GetPostsByCategoryResponse>;