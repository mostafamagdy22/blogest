using MediatR;

namespace blogest.application.Features.handlers.Comments;

public class GetCommentByIdHandler : IRequestHandler<GetCommentByIdQuery, GetCommentByIdResponse>
{
    private readonly ICommentsQueryRepository _commentsQueryReository;
    public GetCommentByIdHandler(ICommentsQueryRepository commentsQueryRepository)
    {
        _commentsQueryReository = commentsQueryRepository;
    }
    public async Task<GetCommentByIdResponse> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
    {
        GetCommentByIdResponse response = await _commentsQueryReository.GetCommentById(request.CommentId);
        return response;
    }
}