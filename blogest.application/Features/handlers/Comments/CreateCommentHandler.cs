using AutoMapper;
using MediatR;

namespace blogest.application.Features.handlers.Comments;

public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, CreateCommentResponse>
{
    private readonly ICommentsCommandRepository _repository;
    private readonly IMapper _mapper;
    public CreateCommentHandler(ICommentsCommandRepository repository, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }
    public async Task<CreateCommentResponse> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        Comment comment = _mapper.Map<Comment>(request);
        CreateCommentResponse response = await _repository.CreateComment(comment);

        return response;
    }
}