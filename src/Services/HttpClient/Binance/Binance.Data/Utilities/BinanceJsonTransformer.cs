using Binance.Data.Dto;
using Hvt.Utilities;
using System.Globalization;
using System.Text.Json;

namespace Binance.Data.Utilities
{
    public class BinanceJsonTransformer
    {
        public static List<KlineDto> TransformKlineJson(string json)
        {
            List<List<JsonElement>> rawData = Serializer.Deserialize<List<List<JsonElement>>>(json);

            List<KlineDto> klines = rawData.Select(item => new KlineDto
            {
                OpenTime = item[0].GetInt64(),

                Open = Convert.ToDecimal(item[1].GetString(), CultureInfo.InvariantCulture),
                High = Convert.ToDecimal(item[2].GetString(), CultureInfo.InvariantCulture),
                Low = Convert.ToDecimal(item[3].GetString(), CultureInfo.InvariantCulture),
                Close = Convert.ToDecimal(item[4].GetString(), CultureInfo.InvariantCulture),
                Volume = Convert.ToDecimal(item[5].GetString(), CultureInfo.InvariantCulture),
                CloseTime = item[6].GetInt64(),
                QuoteAssetVolume = Convert.ToDecimal(item[7].GetString(), CultureInfo.InvariantCulture),
                NumberOfTrades = item[8].GetInt32(),
                TakerBuyBaseVolume = Convert.ToDecimal(item[9].GetString(), CultureInfo.InvariantCulture),
                TakerBuyQuoteVolume = Convert.ToDecimal(item[10].GetString(), CultureInfo.InvariantCulture),
                Ignore = item[11].GetString()
            }).ToList();

            return klines;
        }
    }
}
