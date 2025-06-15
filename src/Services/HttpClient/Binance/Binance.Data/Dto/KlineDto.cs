﻿namespace Binance.Data.Dto
{
    public class KlineDto
    {
        public long OpenTime { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
        public long CloseTime { get; set; }
        public decimal QuoteAssetVolume { get; set; }
        public int NumberOfTrades { get; set; }
        public decimal TakerBuyBaseVolume { get; set; }
        public decimal TakerBuyQuoteVolume { get; set; }
        public string Ignore { get; set; } = string.Empty;

        public decimal GetMediumPrice()
        {
            return (Open + Close) / 2;
        }
    }
}
