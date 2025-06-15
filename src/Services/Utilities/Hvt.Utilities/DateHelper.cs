namespace Hvt.Utilities
{
    public class DateHelper
    {
        public static long GetTimestampForSpecificDate(DateTime specificDate)
        {
            return new DateTimeOffset(specificDate).ToUnixTimeMilliseconds();
        }

        public static DateTime ConvertTimestampToDateTime(long timestamp)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
        }
    }
}
