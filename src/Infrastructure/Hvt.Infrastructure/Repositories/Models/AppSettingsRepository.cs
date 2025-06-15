using Hvt.Data.Enums;
using Hvt.Data.Models;
using Hvt.Infrastructure.Repositories.Contract;
using Hvt.Infrastructure.Repositories.DbContext;

namespace Hvt.Infrastructure.Repositories.Models
{
    public class AppSettingsRepository(HvtTradeDbContext repositoryContext) : RepositoryBase<AppSetting, HvtTradeDbContext>(repositoryContext), IAppSettingsRepository
    {
        public decimal GetCurrentCapital()
        {
            AppSetting? appSetting =
                FindByCondition(a => a.SettingKey == AppSettingKey.CurrentCapital, false).FirstOrDefault();
            if (appSetting == null)
            {
                return 0;
            }
            if (decimal.TryParse(appSetting.SettingValue, out decimal currentCapital))
            {
                return currentCapital;
            }
            else
            {
                throw new FormatException($"Invalid format for CurrentCapital: {appSetting.SettingValue}");
            }
        }

        public void SetCurrentCapital(decimal value)
        {
            AppSetting? appSetting =
                FindByCondition(a => a.SettingKey == AppSettingKey.CurrentCapital, true).FirstOrDefault();
            if (appSetting == null)
            {
                return;
            }
            appSetting.SettingValue = value.ToString("G29"); 
            Update(appSetting);
        }
    }
}
