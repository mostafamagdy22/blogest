using blogest.application.DTOs.responses.Search;
using blogest.application.Features.queries.Search;
using blogest.application.Interfaces.services;
using blogest.domain.Constants;
using MediatR;

namespace blogest.application.Features.handlers.Search;

public class SearchPostHandler : IRequestHandler<SearchQuery, SearchQueryResponse>
{
    private readonly ISearchService _searchService;
    public SearchPostHandler(ISearchService searchService)
    {
        _searchService = searchService;
    }
    public async Task<SearchQueryResponse> Handle(SearchQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<(string Id, GetPostResponse Document)> response = await _searchService.SearchAsync<GetPostResponse>(ElasticsearchIndecis.articles,request.field ,request.query, request.pageNumber, request.pageSize);

        SearchQueryResponse responseDto = new SearchQueryResponse
        {
            Posts = response.Select(s => s.Document).ToList()
        };

        return responseDto;
    }
}