namespace ChromaMusicSearch.Utils
{
    public static class VectorUtils
    {
        public static ReadOnlyMemory<float> CreateEmbeddingFromText(string text)
        {
            // Burada bir metni alıp, örneğin bir model ile vektör haline getirebiliriz.
            // Şu an sadece basit bir örnek gösteriliyor.
            return new ReadOnlyMemory<float>(new float[] { 0.1f, 0.2f }); // Örnek vektör
        }
    }
}
