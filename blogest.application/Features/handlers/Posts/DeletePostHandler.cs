using blogest.application.Interfaces.services;
using blogest.domain.Constants;
using MediatR;

namespace blogest.application.Features.handlers.Posts;

public class DeletePostHandler : IRequestHandler<DeletePostCommand, DeletePostResponse>
{
    private readonly IPostsCommandRepository _postsRepository;
    private readonly ISearchService _searchService;
    public DeletePostHandler(ISearchService searchService, IPostsCommandRepository postsRepository)
    {
        _searchService = searchService;
        _postsRepository = postsRepository;
    }
    public async Task<DeletePostResponse> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        DeletePostResponse response = await _postsRepository.DeletePost(request.postId);

        string documentId = await _searchService.GetDocumentId<GetPostResponse>(request.postId);

        await _searchService.DeleteAsync(ElasticsearchIndecis.articles,documentId);

        return response;
    }
}