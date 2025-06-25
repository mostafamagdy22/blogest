using MediatR;

namespace blogest.application.Features.handlers.Posts;

public class DeletePostsByUserHandler : IRequestHandler<DeletePostsByUserCommand, DeletePostResponse>
{
    private readonly IPostsCommandRepository _postsCommandRepository;
    public DeletePostsByUserHandler(IPostsCommandRepository postsCommandRepository)
    {
        _postsCommandRepository = postsCommandRepository;
    }
    public async Task<DeletePostResponse> Handle(DeletePostsByUserCommand request, CancellationToken cancellationToken)
    {
        DeletePostResponse response = await _postsCommandRepository.DeletePostsByUser(request.userId);

        if (response == null)
            throw new Exception(response.Message ?? "An error occurred while deleting posts by user.");

        return response;
    }
}