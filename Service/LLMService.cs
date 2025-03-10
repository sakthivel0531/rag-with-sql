using RagWithSql.Interface;
using RagWithSql.Model;
using System.Text;
using System.Text.Json;

namespace RagWithSql.Service
{
    public class LLMService : ILLMService
    {
        private readonly string apiKey = "your-azure-openai-key";
        private readonly string chatEndpoint = "https://your-openai-endpoint.openai.azure.com/v1/chat/completions";

        public LLMService()
        {
                
        }
        public async Task<string> GenerateResponseAsync(string userQuery, List<Transaction> transactions)
        {
            string retrievedData = string.Join("\n", transactions.Select(t => $"Customer {t.CustomerID} bought {t.ItemPurchased} for ${t.Price}"));

            var requestBody = new
            {
                model = "gpt-4",
                messages = new[]
                {
                new { role = "system", content = "You are a helpful assistant." },
                new { role = "user", content = $"The user asked: \"{userQuery}\". Here are the relevant transactions:\n{retrievedData}\nGenerate a human-like response summarizing the retrieved transactions." }
            }
            };

            string json = JsonSerializer.Serialize(requestBody);

            using HttpClient client = new();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var response = await client.PostAsync(chatEndpoint, new StringContent(json, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error generating LLM response");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(responseBody);

            return result.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
        }


    }
}
