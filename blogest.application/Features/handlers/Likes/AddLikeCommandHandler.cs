using blogest.application.Features.commands.Likes;
using blogest.application.Interfaces.repositories.Likes;
using MediatR;

namespace blogest.application.Features.handlers.Likes;

public class AddUnLikeCommandHandler : IRequestHandler<AddLikeCommand, AddLikeResponse>
{
    private readonly ILikesCommandRepository _likesCommandRepository;
    public AddUnLikeCommandHandler(ILikesCommandRepository likesCommandRepository)
    {
        _likesCommandRepository = likesCommandRepository;
    }
    public async Task<AddLikeResponse> Handle(AddLikeCommand request, CancellationToken cancellationToken)
    {
        AddLikeResponse response = await _likesCommandRepository.AddLike(request.postId);

        return response;
    }
}