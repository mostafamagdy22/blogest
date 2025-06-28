using MediatR;

namespace blogest.application.Features.handlers.Posts;

public class GetPostsByUserIdHandler : IRequestHandler<GetPostsByUserIdQuery, GetPostsByCategoryResponse>
{
    private readonly IPostsQueryRepository _postsQueryRepository;
    public GetPostsByUserIdHandler(IPostsQueryRepository postsQueryRepository)
    {
        _postsQueryRepository = postsQueryRepository;
    }
    public async Task<GetPostsByCategoryResponse> Handle(GetPostsByUserIdQuery request, CancellationToken cancellationToken)
    {
        GetPostsByCategoryResponse response = await _postsQueryRepository.GetPostsByUser(request);

        return response;
    }
}