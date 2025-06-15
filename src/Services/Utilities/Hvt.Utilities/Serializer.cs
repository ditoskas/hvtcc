using System.Text.Json;

namespace Hvt.Utilities
{
    public class Serializer
    {
        public static string Serialize<T>(T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            return JsonSerializer.Serialize(obj);
        }

        public static T Deserialize<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentNullException(nameof(json));
            return JsonSerializer.Deserialize<T>(json)
                   ?? throw new InvalidOperationException("Deserialization returned null.");
        }
    }
}
