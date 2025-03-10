using RagWithSql.Model;
using System.Data.SqlClient;

namespace RagWithSql.Repository
{
    public class TransactionRepository
    {
        private static readonly string connectionString = "Server=your-server;Database=your-db;User Id=your-user;Password=your-password;";

        public static async Task Insert(Transaction transaction)
        {
            string embeddingString = string.Join(",", transaction.Embedding);

            using SqlConnection connection = new(connectionString);
            await connection.OpenAsync();

            string query = "INSERT INTO Transactions (TransactionID, CustomerID, ItemPurchased, Price, Embedding) VALUES (@TransactionID, @CustomerID, @ItemPurchased, @Price, @Embedding)";

            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@TransactionID", transaction.TransactionID);
            command.Parameters.AddWithValue("@CustomerID", transaction.CustomerID);
            command.Parameters.AddWithValue("@ItemPurchased", transaction.ItemPurchased);
            command.Parameters.AddWithValue("@Price", transaction.Price);
            command.Parameters.AddWithValue("@Embedding", embeddingString);

            await command.ExecuteNonQueryAsync();
        }
    }
}
