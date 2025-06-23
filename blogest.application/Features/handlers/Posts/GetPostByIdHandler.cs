using MediatR;

namespace blogest.application.Features.handlers
{
    public class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, GetPostResponse>
    {
        private readonly IPostsQueryRepository _postsQueryRepository;
        public GetPostByIdHandler(IPostsQueryRepository postsQueryRepository)
        {
            _postsQueryRepository = postsQueryRepository;
        }
        public async Task<GetPostResponse> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await _postsQueryRepository.GetPostByIdAsync(request.postId);

            if (post is null)
                return null;

            return post;
        }
    }
}