using Hvt.Utilities;
using Polygon.Data.Responses;
using Xunit;

namespace Polygon.Tests
{
    public class DeserializeResponses
    {
        public const string RsiResponseJson =
            @"{""results"":{""underlying"":{""url"":""https://api.polygon.io/v2/aggs/ticker/BTC/range/1/minute/1063281600000/1749915496217?limit=66\u0026sort=desc""},""values"":[{""timestamp"":1749853380000,""value"":65.00329352333333}]},""status"":""OK"",""request_id"":""d6109f7127d90c3fe0c1214fec08d0e3"",""next_url"":""https://api.polygon.io/v1/indicators/rsi/BTC?cursor=YWRqdXN0ZWQ9dHJ1ZSZhcD0lN0IlMjJ2JTIyJTNBMCUyQyUyMm8lMjIlM0EwJTJDJTIyYyUyMiUzQTQ2LjgyJTJDJTIyaCUyMiUzQTAlMkMlMjJsJTIyJTNBMCUyQyUyMnQlMjIlM0ExNzQ5ODUzMzIwMDAwJTdEJmFzPSZleHBhbmRfdW5kZXJseWluZz1mYWxzZSZsaW1pdD0xJm9yZGVyPWRlc2Mmc2VyaWVzX3R5cGU9Y2xvc2UmdGltZXNwYW49bWludXRlJnRpbWVzdGFtcC5sdD0xNzQ5ODUzMzgwMDAwJndpbmRvdz0xNA""}";
        [Fact]
        public void DeserializeRsiResponse_ShouldReturnValidObject()
        {
            RsiResponse responseObj = Serializer.Deserialize<RsiResponse>(RsiResponseJson);
            Assert.NotNull(responseObj);
            Assert.NotNull(responseObj.Results);
            Assert.NotNull(responseObj.Results.Underlying);
            Assert.NotNull(responseObj.Results.Values);
        }

        // Add more methods for different response types as needed
    }
}
