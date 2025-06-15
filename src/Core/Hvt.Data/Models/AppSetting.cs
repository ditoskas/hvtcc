using Hvt.Data.Models.Base;

namespace Hvt.Data.Models
{
    public class AppSetting : EntityWithTimestamps
    {
        public string SettingKey{ get; set; } = string.Empty;
        public string SettingValue{ get; set; } = string.Empty;
    }
}
