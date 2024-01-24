namespace YahooFinanceApp.Models
{
    public class StockModel
    {
        public required string Symbol { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; } = 0M;
        // Diğer özellikler eklenir...
    }
}
