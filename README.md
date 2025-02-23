![image](https://github.com/user-attachments/assets/598d01cd-bd99-44bc-9659-2198078da05a)

# Chroma C# SDK ile AI Uygulamaları

Bu proje, **Chroma C# SDK** kullanarak yapay zeka uygulamaları geliştirmek için bir örnek sunmaktadır. Chroma, gömülü vektörleri depolama, vektör arama, metaveri filtreleme ve çok modlu veri alma gibi özellikleriyle güçlü bir açık kaynak veritabanıdır. Bu proje, müzik verileri kullanarak vektör arama ve semantik benzerlik tabanlı sorgularla ilgili örnekler sunmaktadır.

## Başlarken

Bu kılavuz, **Chroma** ile yapay zeka uygulamalarınızı geliştirirken kullanabileceğiniz bir proje yapısına dair detayları sunmaktadır. Bu projeyi bilgisayarınızda çalıştırabilmek için aşağıdaki adımları izleyebilirsiniz.

### Gereksinimler

1. **.NET SDK**: [İndir](https://dotnet.microsoft.com/download)
   - Bu proje .NET 9.0 sürümü ile uyumludur.
2. **Docker**: [İndir](https://www.docker.com/products/docker-desktop)
   - Chroma'yı çalıştırmak için Docker gereklidir.

### Chroma Sunucusunu Çalıştırma

Chroma'yı çalıştırmak için aşağıdaki adımları takip edebilirsiniz:

1. **Docker'ı Başlatın**:
   Docker Desktop uygulamasını açın ve çalıştığından emin olun.

2. **Chroma Docker İmajını Çekin**:
   Chroma'nın Docker imajını çekmek için terminal veya komut istemcisinde aşağıdaki komutu çalıştırın:
   ```bash
   docker pull chromadb/chroma
   ```

3. **Chroma Sunucusunu Çalıştırın**:
   Chroma'yı başlatmak için aşağıdaki komutu kullanın:
   ```bash
   docker run -p 8000:8000 chromadb/chroma
   ```
   Bu komut, Chroma'yı yerel makinenizde **http://localhost:8000** adresinde çalıştıracaktır.

Eğer bir uzak sunucu kullanıyorsanız, bağlantı için servis sağlayıcınızdan URI'yi almanız gerekecektir.

## Projeyi Klonlama

Projenizi yerel bilgisayarınıza klonlamak için aşağıdaki adımları izleyin:

1. **Git İle Projeyi Klonlayın**:
   ```bash
   https://github.com/KardelRuveyda/chromadb-dotnet-sample
   cd chromadb-dotnet
   ```

2. **NuGet Paketlerini Yükleyin**:
   Projeye gerekli bağımlılıkları eklemek için şu komutu çalıştırın:
   ```bash
   dotnet restore
   ```

### Uygulamayı Çalıştırma

Projeyi çalıştırmak için terminalde aşağıdaki komutu kullanabilirsiniz:
```bash
dotnet run
```

Bu komut, Chroma sunucusuna bağlanarak müzik veritabanına şarkı ekleyecek ve vektör arama ile "rahatlatıcı bir şarkı" ifadesine yakın şarkıları arayacaktır.

## Chroma Kullanarak Veri Ekleme ve Arama

Aşağıda projenizdeki ana adımlar ve kod örneklerini bulabilirsiniz.

### 1. **Chroma İstemcisi Oluşturma**
   Chroma veritabanına bağlanmak için istemciyi şu şekilde oluşturabilirsiniz:
   ```csharp
   using ChromaDB.Client;
   using ChromaMusicSearch.Services;

   var chromaClientService = new ChromaClientService("http://localhost:8000/api/v1/");
   var collection = await chromaClientService.GetCollectionAsync("music");
   ```

### 2. **Koleksiyon Oluşturma**
   Müzik verilerini saklamak için bir koleksiyon oluşturun:
   ```csharp
   var chromaCollectionService = new ChromaCollectionService(collection, new ChromaConfigurationOptions("http://localhost:8000/api/v1/"), new HttpClient());
   ```

### 3. **Veri Ekleme**
   Şarkıların embeddings'lerini ve metadatalarını içeren veriyi ekleyin:
   ```csharp
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
   ```

### 4. **Vektör Arama ile Müzik Bulma**
   Kullanıcının sorgusuna benzer şarkıları vektör arama ile bulun:
   ```csharp
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
   ```

### Örnek Çıktı:
```
Title: Billie Jean Distance: 0.023739595
Title: Imagine Distance: 0.027785346
```

Bu çıktıda, verilen sorguya en yakın iki şarkı (Billie Jean ve Imagine) ve mesafeleri gösterilmektedir.

## Katkıda Bulunma

Katkı sağlamak isteyenler için:

1. Bu depoyu forklayın.
2. Yeni bir dal oluşturun (`git checkout -b feature-isim`).
3. Değişikliklerinizi yapın ve commit edin (`git commit -am 'Yeni özellik ekle'`).
4. Dalınızı uzak depoya gönderin (`git push origin feature-isim`).
5. Bir pull request açın.

## Lisans

Bu proje [MIT Lisansı](https://opensource.org/licenses/MIT) ile lisanslanmıştır.


