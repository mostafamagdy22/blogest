using MediatR;

namespace blogest.application.Features.handlers.Comments;

public class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand, DeleteCommentResponse>
{
    private readonly ICommentsCommandRepository _commentsCommandRepository;
    public DeleteCommentHandler(ICommentsCommandRepository commentsCommandRepository)
    {
        _commentsCommandRepository = commentsCommandRepository;
    }
    public async Task<DeleteCommentResponse> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        DeleteCommentResponse response = await _commentsCommandRepository.DeleteComment(request.CommentId);

        return response;
    }
}