using RagWithSql.Model;

namespace RagWithSql.Interface
{
    public interface ILLMService
    {
        Task<string> GenerateResponseAsync(string userQuery, List<Transaction> transactions);
    }
}
