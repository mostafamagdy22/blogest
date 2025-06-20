using AutoMapper;
using blogest.application.DTOs.responses;
using blogest.application.Features.commands;
using blogest.application.Interfaces.repositories;
using blogest.domain.Entities;
using MediatR;

namespace blogest.application.Features.handlers
{
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
            Post post = _mapper.Map<Post>(postCommand);
            await _postsRepository.AddAsync(post);
            if (post.PostId != null)
                return new CreatePostResponseDto(post.PostId, true);
            else
                return new CreatePostResponseDto(null, false);
        }
    }
}