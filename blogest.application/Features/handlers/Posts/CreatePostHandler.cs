using AutoMapper;
using MediatR;

namespace blogest.application.Features.handlers.Posts;

public class CreatePostHandler : IRequestHandler<CreatePostCommand, CreatePostResponseDto>
{
    private readonly IPostsCommandRepository _postsRepository;
    private readonly IMapper _mapper;
    public CreatePostHandler(IPostsCommandRepository postsRepository, IMapper mapper)
    {
        _postsRepository = postsRepository;
        _mapper = mapper;
    }
    public async Task<CreatePostResponseDto> Handle(CreatePostCommand postCommand, CancellationToken cancellationToken)
    {
        if (postCommand.CategoryIds.Count == 0)
            return new CreatePostResponseDto("Should add atleast one category",null, false);
        Post post = _mapper.Map<Post>(postCommand);
        await _postsRepository.AddAsync(post,postCommand.CategoryIds);
        if (post.PostId != null)
            return new CreatePostResponseDto("post created successfully",post.PostId, true);
        else
            return new CreatePostResponseDto("something happend while add post",null, false);
    }
}