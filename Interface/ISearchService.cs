using RagWithSql.Model;

namespace RagWithSql.Interface
{
    public interface ISearchService
    {
        Task<List<Transaction>> SearchTransactionsAsync(string query);
    }
}
