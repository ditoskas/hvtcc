namespace Hvt.Data.Dtos
{
    public class StatisticsDto
    {
        public int TotalTrades { get; set; }
        public decimal TotalProfitAndLoss { get; set; }
        public int TotalTradesInProfit { get; set; }
        public int TotalTradesInLoss { get; set; }
    }
}
