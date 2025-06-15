using Hvt.Utilities;
using Polygon.Client.Services;
using Polygon.Data.Enums;
using Polygon.Data.Responses;
using Xunit;

namespace Polygon.Tests
{
    public class IntegrationTests (PolygonClient polygonClient)
    {
        [Fact]
        public async Task GetRsiData_ShouldReturnValidResponse()
        {
            PolygonResponse<RsiResponse?> response = await polygonClient.GetRsi("BTC", TimespanPeriod.Minute);
            // Assert
            Assert.NotNull(response);
            Assert.True(response.Data?.Results.Values.Count == 1);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task GetRsiData_SimulateDataValidResponse()
        {
            DateTime sellRsi = new DateTime(2025, 6, 15, 8, 30, 0, DateTimeKind.Local);
            long timestamp = DateHelper.GetTimestampForSpecificDate(sellRsi);

            // Arrange
            PolygonResponse<RsiResponse?> response = await polygonClient.GetRsi("BTC", TimespanPeriod.Minute, timestamp.ToString());
            // Assert
            Assert.NotNull(response);
            Assert.True(response.Data?.Results.Values.Count == 1);
            Assert.True(response.Result);
        }
    }
}
