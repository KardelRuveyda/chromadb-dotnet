using ChromaDB.Client;
using ChromaMusicSearch.Services;


var chromaClientService = new ChromaClientService("http://localhost:8000/api/v1/");
var collection = await chromaClientService.GetCollectionAsync("music");

// Koleksiyonla bağlantı kurulduktan sonra ChromaCollectionService kullanılır
var chromaCollectionService = new ChromaCollectionService(collection, new ChromaConfigurationOptions("http://localhost:8000/api/v1/"), new HttpClient());

// Şarkı kimlikleri
List<string> songIds = ["1", "2", "3", "4", "5"];

// Vektör verileri (embedding) şarkı açıklamaları
List<ReadOnlyMemory<float>> songEmbeddings = [
    new [] { 0.124f, -0.256f },
    new [] { 0.113f, 0.299f },
    new [] { 0.109f, -0.201f },
    new [] { 0.132f, 0.212f },
    new [] { 0.098f, -0.187f },
];

// Metadata verisi
List<Dictionary<string, object>> metadata = [
    new Dictionary<string, object> { ["Title"] = "Bohemian Rhapsody" },
    new Dictionary<string, object> { ["Title"] = "Shape of You" },
    new Dictionary<string, object> { ["Title"] = "Imagine" },
    new Dictionary<string, object> { ["Title"] = "Hotel California" },
    new Dictionary<string, object> { ["Title"] = "Billie Jean" },
];
// Verileri koleksiyona ekleme
await chromaCollectionService.AddDataAsync(songIds, songEmbeddings, metadata);

// Vektör sorgusu ( Rahatlatıcı Bir Şarkı Önerisini Yapsın? Rahatlatıcı Bir Şarkı ifadesine gelen vektör) 

List<ReadOnlyMemory<float>> queryEmbedding = new List<ReadOnlyMemory<float>> {
            new ReadOnlyMemory<float>(new [] { 0.12217915f, -0.034832448f })
        };

var queryResults = await chromaCollectionService.QueryAsync(queryEmbedding, 2);

// Sonuçları yazdırma
foreach (var result in queryResults)
{
    string title = result.Metadata != null && result.Metadata.ContainsKey("Title") ? (string)result.Metadata["Title"] : string.Empty;
    Console.WriteLine($"Title: {title} Distance: {result.Distance}");
}
