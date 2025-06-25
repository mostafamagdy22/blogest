using MediatR;

namespace blogest.application.Features.handlers.Comments;

public class GetCommentsByPostIdHandler : IRequestHandler<GetCommentsByPostIdQuery, GetCommentsOnPostResponse>
{
    private readonly ICommentsQueryRepository _commentsQueryRepository;
    public GetCommentsByPostIdHandler(ICommentsQueryRepository commentsQueryRepository)
    {
        _commentsQueryRepository = commentsQueryRepository;
    }
    public async Task<GetCommentsOnPostResponse> Handle(GetCommentsByPostIdQuery request, CancellationToken cancellationToken)
    {
        GetCommentsOnPostResponse response = await _commentsQueryRepository.GetCommentsByPostId(request.postId,request.pageNumber,request.pageSize);

        return response;
    }
}