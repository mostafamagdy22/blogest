using MediatR;

namespace blogest.application.Features.handlers;

public class UpdatePostHandler : IRequestHandler<UpdatePostCommand, UpdatePostResponse>
{
    private readonly IPostsCommandRepository _postsRepository;
    public UpdatePostHandler(IPostsCommandRepository postsRepository)
    {
        _postsRepository = postsRepository;
    }
    public async Task<UpdatePostResponse> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        UpdatePostResponse response = await _postsRepository.UpdatePost(request);
        return response;
    }
}
