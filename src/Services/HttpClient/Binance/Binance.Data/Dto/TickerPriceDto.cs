using System.Text.Json.Serialization;

namespace Binance.Data.Dto
{
    public class TickerPriceDto
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;

        [JsonPropertyName("price")]
        public string Price { get; set; } = string.Empty;

        [JsonPropertyName("time")]
        public long Time { get; set; }
    }
}
