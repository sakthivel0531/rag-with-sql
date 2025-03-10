using RagWithSql.Interface;
using RagWithSql.Model;
using System.Data.SqlClient;

namespace RagWithSql.Service
{
    public class SearchService : ISearchService
    {
        private readonly IConfiguration _configuration;
        public SearchService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<List<Transaction>> SearchTransactionsAsync(string query)
        {
            float[] queryEmbedding = await OpenAIService.GenerateEmbeddingAsync(query);
            string embeddingString = string.Join(",", queryEmbedding);

            List<Transaction> results = new();

            using SqlConnection connection = new(_configuration["ConnectionStrings:DefaultConnection"]);
            await connection.OpenAsync();

            string searchQuery = @"
        DECLARE @query_embedding VECTOR(1536) = ?
        SELECT TOP 5 * FROM Transactions
        ORDER BY Embedding <-> @query_embedding;"; // Nearest neighbor search

            using SqlCommand command = new(searchQuery, connection);
            command.Parameters.AddWithValue("@query_embedding", embeddingString);

            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                results.Add(new Transaction
                {
                    TransactionID = reader.GetInt32(0),
                    CustomerID = reader.GetInt32(1),
                    ItemPurchased = reader.GetString(2),
                    Price = reader.GetDecimal(3),
                    Embedding = Array.ConvertAll(reader.GetString(4).Split(','), float.Parse)
                });
            }

            return results;
        }
    }
}
