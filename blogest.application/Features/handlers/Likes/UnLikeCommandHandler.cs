using blogest.application.Features.commands.Likes;
using blogest.application.Interfaces.repositories.Likes;
using MediatR;

namespace blogest.application.Features.handlers.Likes;

public class UnLikeCommandHandler : IRequestHandler<UnLikeCommand, UnLikeResponse>
{
    private readonly ILikesCommandRepository _likesCommandRepository;
    public UnLikeCommandHandler(ILikesCommandRepository likesCommandRepository)
    {
        _likesCommandRepository = likesCommandRepository;
    }
    public async Task<UnLikeResponse> Handle(UnLikeCommand request, CancellationToken cancellationToken)
    {
        UnLikeResponse response = await _likesCommandRepository.UnLike(request.postId);

        return response;
    }
}