using Hvt.Data.Enums;
using Hvt.Data.Models;

namespace Hvt.Infrastructure.Repositories.Contract
{
    public interface IAppSettingsRepository : IRepositoryBase<AppSetting>
    {
        public decimal GetCurrentCapital();
        public void SetCurrentCapital(decimal value);
    }
}
