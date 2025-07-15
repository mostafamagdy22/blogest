using blogest.application.Interfaces.services;
using blogest.domain.Constants;
using MediatR;

namespace blogest.application.Features.handlers;

public class UpdatePostHandler : IRequestHandler<UpdatePostCommand, UpdatePostResponse>
{
    private readonly IPostsCommandRepository _postsRepository;
    private readonly ISearchService _searchService;
    public UpdatePostHandler(ISearchService searchService, IPostsCommandRepository postsRepository)
    {
        _searchService = searchService;
        _postsRepository = postsRepository;
    }
    public async Task<UpdatePostResponse> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        UpdatePostResponse response = await _postsRepository.UpdatePost(request);

        (string,GetPostResponse) search = (await _searchService.SearchAsync<GetPostResponse>(
            ElasticsearchIndecis.articles
            , "postId:" + request.postId.ToString())).FirstOrDefault()!;

        UpdatePostCommand updatedFields = new UpdatePostCommand(Title: response.Title,Content:response.Content,postId:request.postId);
        await _searchService.UpdateDocumentAsync<GetPostResponse,UpdatePostCommand>(ElasticsearchIndecis.articles, search.Item1,updatedFields);

        return response;
    }
}
