using blogest.application.Interfaces.repositories.Likes;
using MediatR;

namespace blogest.application.Features.handlers.Posts;

public class GetUserLikesQueryHandler : IRequestHandler<GetUserLikesQuery, GetUserLikesResponse>
{
    private readonly IPostsQueryRepository _postsQueryRepository;
    public GetUserLikesQueryHandler(IPostsQueryRepository postsQueryRepository)
    {
        _postsQueryRepository = postsQueryRepository;
    }
    public async Task<GetUserLikesResponse> Handle(GetUserLikesQuery request, CancellationToken cancellationToken)
    {
        GetUserLikesResponse response = await _postsQueryRepository.GetUserLikes(request);

        return response;
    }
}