using MediatR;

namespace blogest.application.Features.handlers.Posts;

public class UpdatePostCategoriesCommandHandler : IRequestHandler<UpdatePostCategoriesCommand, UpdatePostCategoriesResponse>
{
    private readonly IPostsCommandRepository _postsCommandRepository;
    public UpdatePostCategoriesCommandHandler(IPostsCommandRepository postsCommandRepository)
    {
        _postsCommandRepository = postsCommandRepository;
    }
    public async Task<UpdatePostCategoriesResponse> Handle(UpdatePostCategoriesCommand request, CancellationToken cancellationToken)
    {
        UpdatePostCategoriesResponse response = await _postsCommandRepository.updatePostCategories(request);
        return response;
    }
}