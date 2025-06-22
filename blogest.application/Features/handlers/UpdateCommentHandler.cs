using blogest.application.DTOs.responses;
using blogest.application.Features.commands;
using blogest.application.Interfaces.repositories;
using MediatR;

namespace blogest.application.Features.handlers;

public class UpdateCommentHandler : IRequestHandler<UpdateCommentCommand, UpdateCommentResponse>
{
    private readonly ICommentsCommandRepository _commentsCommandRepository;
    public UpdateCommentHandler(ICommentsCommandRepository commentsCommandRepository)
    {
        _commentsCommandRepository = commentsCommandRepository;
    }
    public async Task<UpdateCommentResponse> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        UpdateCommentResponse response = await _commentsCommandRepository.UpdateComment(request.CommentId, request.NewContent);

        return response;
    }
}