using System.Text.Json.Serialization;

namespace Polygon.Data.Dto
{
    public class ValueDto
    {
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("value")]
        public decimal Value { get; set; }
    }
}
