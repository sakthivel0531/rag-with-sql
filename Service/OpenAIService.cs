using System.Text;
using System.Text.Json;

namespace RagWithSql.Service
{
    public static class OpenAIService
    {
        private static readonly string apiKey = "your-azure-openai-key";
        private static readonly string embeddingEndpoint = "https://your-openai-endpoint.openai.azure.com/v1/embeddings";

        public static async Task<float[]> GenerateEmbeddingAsync(string text)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var requestBody = new
            {
                input = text,
                model = "text-embedding-ada-002"
            };

            string json = JsonSerializer.Serialize(requestBody);
            var response = await client.PostAsync(embeddingEndpoint, new StringContent(json, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error generating embedding");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(responseBody);
            return result.GetProperty("data")[0].GetProperty("embedding").EnumerateArray()
                          .Select(x => x.GetSingle()).ToArray();
        }
    }
}
