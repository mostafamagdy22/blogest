using blogest.application.Features.queries.Likes;
using blogest.application.Interfaces.repositories.Likes;
using MediatR;

namespace blogest.application.Features.handlers.Likes;

public class GetPostLikesQueryHandler : IRequestHandler<GetPostLikesQuery, GetPostLikesResponse>
{
    private readonly ILikesQueryRepository _likesQueryRepository;
    public GetPostLikesQueryHandler(ILikesQueryRepository likesQueryRepository)
    {
        _likesQueryRepository = likesQueryRepository;
    }
    public async Task<GetPostLikesResponse> Handle(GetPostLikesQuery request, CancellationToken cancellationToken)
    {
        GetPostLikesResponse response = await _likesQueryRepository.GetPostLikes(request.PostId);

        return response;
    }
}