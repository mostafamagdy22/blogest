using blogest.application.Features.commands;
using blogest.application.DTOs.responses;
using MediatR;
using blogest.application.Interfaces.repositories;

namespace blogest.application.Features.handlers;

public class DeletePostHandler : IRequestHandler<DeletePostCommand, DeletePostResponse>
{
    private readonly IPostsCommandRepository _postsRepository;
    public DeletePostHandler(IPostsCommandRepository postsRepository)
    {
        _postsRepository = postsRepository;
    }
    public async Task<DeletePostResponse> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        DeletePostResponse response = await _postsRepository.DeletePost(request.postId);
        return response;
    }
}