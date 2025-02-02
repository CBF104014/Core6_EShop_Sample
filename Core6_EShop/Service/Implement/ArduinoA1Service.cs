using Core6_EShop.Models;
using Core6_EShop.Repository.Implement;
using Core6_EShop.Service.Base;

namespace Core6_EShop.Service.Implement
{
    public class ArduinoA1Service : BaseService<ArduinoA1>
    {
        private ArduinoA1Repository arduinoA1Repository { get; set; }
        public ArduinoA1Service(ArduinoA1Repository arduinoA1Repository) : base(arduinoA1Repository)
        {
            this.arduinoA1Repository = arduinoA1Repository;
        }
        public async Task<ArduinoA1> SelByUserId(string userId)
        {
            return await arduinoA1Repository.SelByUserId(userId);
        }
    }
}
