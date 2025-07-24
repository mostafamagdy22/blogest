using MediatR;

namespace blogest.application.Features.handlers.Posts;

public class GetSavePostsByUserHandler : IRequestHandler<GetSavePostsOfUser, GetPostsByCategoryResponse>
{
    private readonly IPostsQueryRepository _postsQueryRepository;
    public GetSavePostsByUserHandler(IPostsQueryRepository postsQueryRepository)
    {
        _postsQueryRepository = postsQueryRepository;
    }
    public async Task<GetPostsByCategoryResponse> Handle(GetSavePostsOfUser request, CancellationToken cancellationToken)
    {
        GetPostsByCategoryResponse response = await _postsQueryRepository.GetSavePostsByUser(request.userId,request.include,request.pageNumber,request.pageSize);

        return response;
    }
}