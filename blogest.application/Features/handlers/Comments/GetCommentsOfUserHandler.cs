using MediatR;

namespace blogest.application.Features.handlers.Comments;

public class GetCommentsOfUserHandler : IRequestHandler<GetCommentsOfUserQuery, GetCommentsOfUserResponse>
{
    private readonly ICommentsQueryRepository _commentsQueryRepository;
    public GetCommentsOfUserHandler(ICommentsQueryRepository commentsQueryRepository)
    {
        _commentsQueryRepository = commentsQueryRepository;
    }
    public async Task<GetCommentsOfUserResponse> Handle(GetCommentsOfUserQuery request, CancellationToken cancellationToken)
    {
        GetCommentsOfUserResponse response = await _commentsQueryRepository.GetCommentsOfUser(request.userId);
        return response;
    }
}