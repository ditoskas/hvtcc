namespace Binance.Data.Responses
{
    public class BinanceResponse<T>
    {
        public bool Result { get; set; }
        public string? Error { get; set; }
        public T? Data { get; set; }
    }
}
