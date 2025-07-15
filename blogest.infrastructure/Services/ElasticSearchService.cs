using blogest.application.Interfaces.services;
using blogest.domain.Constants;
using Elastic.Clients.Elasticsearch;

namespace blogest.infrastructure.Services;

public class ElasticSearchService : ISearchService
{
    private readonly ElasticsearchClient _client;
    public ElasticSearchService(ElasticsearchClient client)
    {
        _client = client;
    }

    public async Task DeleteAsync(string indexName, string documentId)
    {
        DeleteResponse response = await _client.DeleteAsync<object>(documentId, idx => idx.Index(indexName));

        if (!response.IsValidResponse)
            throw new Exception(ErrorMessages.BadRequest);
    }

    public async Task IndexAsync<T>(T document, string indexName)
    {
        IndexResponse response = await _client.IndexAsync(document, idx => idx.Index(indexName));

        if (!response.IsValidResponse)
        {
            var serverError = response.ElasticsearchServerError?.ToString() ?? "Unknown Elasticsearch error";
            var debug = response.DebugInformation;
            throw new Exception($"Elasticsearch indexing failed:\nError: {serverError}\nDebug: {debug}");
        }
    }

    public async Task<IEnumerable<T>> SearchAsync<T>(string indexName, string query, int pageNumber = 1, int pageSize = 10)
    {
        SearchResponse<T> response = await _client.SearchAsync<T>(s => s
        .Indices(indexName)
        .From((pageNumber - 1) * pageSize)
        .Size(pageSize)
        .Query(q => q
        .QueryString(qs => qs.Query(query))));

        if (!response.IsValidResponse)
            throw new Exception(ErrorMessages.BadRequest);

        return response.Documents;
    }
}