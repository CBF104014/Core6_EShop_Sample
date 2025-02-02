using Core6_EShop.Cls;
using Core6_EShop.Models;
using Core6_EShop.Repository.Base;

namespace Core6_EShop.Repository.Implement
{
    public class ArduinoA1Repository : BaseRepository<ArduinoA1>
    {
        public ArduinoA1Repository() { }
        public async Task<ArduinoA1> SelByUserId(string userId)
        {
            return await SelFirstAsync<ArduinoA1>("UserId=@userId", new { userId });
        }
    }
}
