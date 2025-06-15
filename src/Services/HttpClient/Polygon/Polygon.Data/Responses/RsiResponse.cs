using Polygon.Data.Dto;
using System.Text.Json.Serialization;

namespace Polygon.Data.Responses
{
    public class RsiResponse
    {
        [JsonPropertyName("next_url")]
        public string NextUrl { get; set; } = string.Empty;

        [JsonPropertyName("request_id")]
        public string RequestId { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public ResultsDto Results { get; set; } = new ResultsDto();

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }
}
