using Hvt.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binance.Data.Dto;
using Xunit;

namespace Binance.Tests
{
    public class DeserializeResponses
    {
        private const string TickerPriceResponse =
            @"{""symbol"":""BTCUSDT"",""price"":""104620.10"",""time"":1749921943218}";
        [Fact]
        public void DeserializeRsiResponse_ShouldReturnValidObject()
        {
            TickerPriceDto responseObj = Serializer.Deserialize<TickerPriceDto>(TickerPriceResponse);
            Assert.NotNull(responseObj);
            Assert.Equal("104620.10", responseObj.Price);
            Assert.Equal("BTCUSDT", responseObj.Symbol);
            Assert.Equal(1749921943218, responseObj.Time);
        }
    }
}
