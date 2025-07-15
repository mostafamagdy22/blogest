namespace blogest.application.Interfaces.services;

public interface ISearchService
{
    public Task IndexAsync<T>(T document, string indexName);
    public Task<IEnumerable<(string Id, T Document)>> SearchAsync<T>(string indexName, string query, int pageNumber = 1, int pageSize = 10);
    public Task DeleteAsync(string indexName, string documentId);
    public Task UpdateDocumentAsync<TDoc,TPartial>(string indexName,string documentId,TPartial updatedFields);
}