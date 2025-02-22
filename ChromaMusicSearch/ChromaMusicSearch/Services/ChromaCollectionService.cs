using ChromaDB.Client;
using ChromaDB.Client.Models;

namespace ChromaMusicSearch.Services
{
    public class ChromaCollectionService
    {
        private readonly ChromaCollectionClient _collectionClient;

        public ChromaCollectionService(ChromaCollection collection, ChromaConfigurationOptions configOptions, HttpClient httpClient)
        {
            _collectionClient = new ChromaCollectionClient(collection, configOptions, httpClient);
        }

        // Verileri koleksiyona ekleme
        public async Task AddDataAsync(List<string> ids, List<ReadOnlyMemory<float>> embeddings, List<Dictionary<string, object>> metadata)
        {
            await _collectionClient.Add(ids, embeddings, metadata);
        }

        // Vektör sorgusu yapma
        public async Task<IEnumerable<ChromaCollectionQueryEntry>> QueryAsync(List<ReadOnlyMemory<float>> queryEmbedding, int nResults)
        {
            var queryResults = await _collectionClient.Query(
                queryEmbeddings: queryEmbedding,
                nResults: nResults,
                include: ChromaQueryInclude.Metadatas | ChromaQueryInclude.Distances);

            return queryResults.SelectMany(result => result);
        }
    }
}
