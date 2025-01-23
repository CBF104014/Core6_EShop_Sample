using Core6_EShop.Models;
using Core6_EShop.Repository.Implement;
using Core6_EShop.Service.Base;

namespace Core6_EShop.Service.Implement
{
    public class SettingService : BaseService<Setting>
    {
        private SettingRepository settingRepository { get; set; }
        public SettingService(SettingRepository settingRepository) : base(settingRepository)
        {
            this.settingRepository = settingRepository;
        }
    }
}
