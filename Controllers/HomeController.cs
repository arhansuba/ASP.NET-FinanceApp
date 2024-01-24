// HomeController.cs

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YahooFinanceApp.Models;

namespace YahooFinanceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private const string AlphaVantageApiKey = "API key"; // Alpha Vantage API anahtarını buraya ekleyin

        [HttpGet]
        [Route("GetStockData")]
        public async Task<IActionResult> GetStockData([FromQuery] string symbol)
        {
            if (string.IsNullOrEmpty(symbol))
            {
                return BadRequest("Symbol cannot be empty");
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Alpha Vantage API'den stok fiyatlarını çekmek için istek oluşturuluyor
                    var apiUrl = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={AlphaVantageApiKey}";
                    var response = await httpClient.GetStringAsync(apiUrl);

                    // API yanıtı JSON olarak deserialize ediliyor
                    var stockData = JsonConvert.DeserializeObject<AlphaVantageResponse>(response);

                    if (stockData?.GlobalQuote != null)
                    {
                        // API'den gelen verileri kullanarak StockModel oluşturuluyor
                        var result = new StockModel
                        {
                            Symbol = stockData.GlobalQuote.Symbol,
                            Name = stockData.GlobalQuote.Symbol, // Alpha Vantage API'nin bu örnekte isim bilgisi içermiyor, bu yüzden sembolü kullanıyoruz.
                            Price = stockData.GlobalQuote.Price
                        };

                        return Ok(result);
                    }
                    else
                    {
                        return NotFound("Stock data not found");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        private class AlphaVantageResponse
        {
            [JsonProperty("Global Quote")]
            public AlphaVantageGlobalQuote? GlobalQuote { get; set; }
        }

        private class AlphaVantageGlobalQuote
        {
            [JsonProperty("01. symbol")]
            public required string Symbol { get; set; }

            [JsonProperty("05. price")]
            public decimal Price { get; set; }
        }
    }
}


