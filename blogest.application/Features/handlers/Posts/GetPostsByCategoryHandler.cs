using MediatR;

namespace blogest.application.Features.handlers.Posts;

public class GetPostsByCategoryHandler : IRequestHandler<GetPostsByCategoryQuery, GetPostsByCategoryResponse>
{
    private readonly IPostsQueryRepository _postsQueryRepository;
    public GetPostsByCategoryHandler(IPostsQueryRepository postsQueryRepository)
    {
        _postsQueryRepository = postsQueryRepository;
    }
    public async Task<GetPostsByCategoryResponse> Handle(GetPostsByCategoryQuery request, CancellationToken cancellationToken)
    {
        GetPostsByCategoryResponse response = await _postsQueryRepository.GetPostsByGategory(request.categoryId,request.include,request.pageNumber,request.pageSize);

        return response;
    }
}