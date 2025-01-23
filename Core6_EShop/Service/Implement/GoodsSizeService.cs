using Core6_EShop.Models;
using Core6_EShop.Repository.Implement;
using Core6_EShop.Service.Base;

namespace Core6_EShop.Service.Implement
{
    public class GoodsSizeService : BaseService<GoodsSize>
    {
        private GoodsSizeRepository goodsSizeRepository { get; set; }
        public GoodsSizeService(GoodsSizeRepository goodsSizeRepository) : base(goodsSizeRepository)
        {
            this.goodsSizeRepository = goodsSizeRepository;
        }
    }
}
