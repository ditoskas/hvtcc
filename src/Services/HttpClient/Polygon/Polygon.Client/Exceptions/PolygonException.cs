using Polygon.Data.Responses;

namespace Polygon.Client.Exceptions
{
    public class PolygonException(string message) : Exception(message)
    {
        public PolygonResponse<object> ToPolygonResponse()
        {
            return new PolygonResponse<object>()
            {
                Result = false,
                Error = Message,
                Data = null
            };
        }
    }
}
