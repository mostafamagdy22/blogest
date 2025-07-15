using blogest.application.DTOs.responses.Search;
using MediatR;

namespace blogest.application.Features.queries.Search;

public record SearchQuery(string field,string query, int pageNumber = 1, int pageSize = 10) : IRequest<SearchQueryResponse>;