using ChromaDB.Client;
using ChromaDB.Client.Models;

namespace ChromaMusicSearch.Services
{
    public class ChromaClientService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly ChromaClient _client;

        public ChromaClientService(string uri)
        {
            var configOptions = new ChromaConfigurationOptions(uri);
            _client = new ChromaClient(configOptions, _httpClient);
        }

        // Connect to or create a collection
        public async Task<ChromaCollection> GetCollectionAsync(string collectionName)
        {
            var collection = await _client.GetOrCreateCollection(collectionName);
            return collection;
        }
    }
}
