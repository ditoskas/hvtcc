using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binance.Client.Services;
using Binance.Data.Dto;
using Binance.Data.Enums;
using Binance.Data.Responses;
using Xunit;

namespace Binance.Tests
{
    public class IntegrationTests(BinanceClient binanceClient)
    {
        [Fact]
        public async Task GetTickerPrice_ShouldReturnValidResponse()
        {
            BinanceResponse<TickerPriceDto> response = await binanceClient.GetTickerPrice(Symbol.BtcUsdt);
            Assert.NotNull(response);
            Assert.True(Convert.ToDecimal(response.Data?.Price) > 0);
            Assert.Equal(Symbol.BtcUsdt, response.Data?.Symbol);
        }
    }
}
