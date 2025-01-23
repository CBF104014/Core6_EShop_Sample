using Core6_EShop.Models;
using Core6_EShop.ViewModel;

namespace Core6_EShop.Dto
{
    public class GoodsRelationManageDto
    {
        public GoodsRelationViewModel goodsRelationData { get; set; }
        public IEnumerable<GoodsSize> goodsSizeDatas { get; set; }
    }
}
