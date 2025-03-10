namespace RagWithSql.Model
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public int CustomerID { get; set; }
        public string ItemPurchased { get; set; }
        public decimal Price { get; set; }
        public float[] Embedding { get; set; }
    }
}
