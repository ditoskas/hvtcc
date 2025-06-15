namespace Polygon.Data.Responses
{
    public class PolygonResponse<T>
    {
        public bool Result { get; set; }
        public string? Error { get; set; }
        public T? Data { get; set; }
    }
}
