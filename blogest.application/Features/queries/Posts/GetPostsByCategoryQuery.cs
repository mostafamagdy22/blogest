using MediatR;

namespace blogest.application.Features.queries.Posts;

public record GetPostsByCategoryQuery(int categoryId,int pageNumber = 1,int pageSize = 10,string? include = "") : IRequest<GetPostsByCategoryResponse>;