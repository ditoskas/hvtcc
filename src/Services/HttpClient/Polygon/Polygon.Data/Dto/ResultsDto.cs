using System.Text.Json.Serialization;

namespace Polygon.Data.Dto
{
    public class ResultsDto
    {
        [JsonPropertyName("underlying")]
        public UnderlyingDto Underlying { get; set; } = new UnderlyingDto();

        [JsonPropertyName("values")]
        public List<ValueDto> Values { get; set; } = new List<ValueDto>();
    }
}
