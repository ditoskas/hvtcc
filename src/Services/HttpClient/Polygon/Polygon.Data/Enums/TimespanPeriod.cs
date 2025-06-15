namespace Polygon.Data.Enums
{
    public abstract class TimespanPeriod
    {
        public const string Minute = "minute";
        public const string Hour = "hour";
        public const string Day = "day";
        public const string Week = "week";
        public const string Month = "month";
        public const string Quarter = "quarter";
        public const string Year = "year";

        public static bool IsValid(string value)
        {
            return value == Minute || value == Hour || value == Day ||
                   value == Week || value == Month || value == Quarter ||
                   value == Year;
        }
    }
}
