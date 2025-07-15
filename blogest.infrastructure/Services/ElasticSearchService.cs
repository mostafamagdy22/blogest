using blogest.application.Interfaces.services;
using blogest.domain.Constants;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;

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

    public async Task<string> GetDocumentId<T>(Guid postId) where T : class
    {
        SearchResponse<T> result = await _client.SearchAsync<T>(s =>
        s.Indices(ElasticsearchIndecis.articles)
        .Size(1)
        .Query(new QueryStringQuery
        {
            Query = $"postId:{postId}"
        }));

        var hit = result.Hits.FirstOrDefault();
        return hit?.Id;
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

    public async Task<IEnumerable<(string Id, T Document)>> SearchAsync<T>(string indexName, string field, string query, int pageNumber = 1, int pageSize = 10) where T : class
    {
        SearchResponse<T> response = await _client.SearchAsync<T>(s => s
        .Indices(indexName)
        .From((pageNumber - 1) * pageSize)
        .Size(pageSize)
        .Query(new MatchQuery
        {
            Field = new Field(field),
            Query = query,
            Fuzziness = "AUTO",
            PrefixLength = 1,
            Operator = Operator.And
        }));

        if (!response.Hits.Any() || !response.IsValidResponse)
            return Enumerable.Empty<(string Id, T Document)>();

        return response.Hits.Select(h => (h.Id, h.Source))!;
    }

    public async Task UpdateDocumentAsync<TDoc, TPartial>(string indexName, string documentId, TPartial updatedFields)
    {
        var response = await _client.UpdateAsync(new UpdateRequest<TDoc, TPartial>(indexName, documentId)
        {
            Doc = updatedFields
        });

        if (!response.IsValidResponse)
            throw new NotImplementedException(ErrorMessages.BadRequest);
    }
}