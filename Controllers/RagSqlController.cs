using Microsoft.AspNetCore.Mvc;
using RagWithSql.Interface;
using RagWithSql.Model;
using RagWithSql.Repository;
using RagWithSql.Service;

namespace RagWithSql.Controllers
{
    public class RagSqlController : ControllerBase
    {
        private readonly ISearchService _searchService;
        public RagSqlController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("find")]
        public async Task<IActionResult> Find(string queryText)
        {
            var response = await _searchService.SearchTransactionsAsync(queryText);
            return Ok(response);
        }

        [HttpPost("add-transaction")]
        public async Task<IActionResult> Add(Transaction transaction)
        {
            transaction.Embedding = await OpenAIService.GenerateEmbeddingAsync($"Customer {transaction.CustomerID} bought {transaction.ItemPurchased} for {transaction.Price}");
            TransactionRepository.Insert(transaction);
            return Ok(StatusCodes.Status200OK);
        }

    }
}
