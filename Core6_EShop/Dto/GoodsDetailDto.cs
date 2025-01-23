using Core6_EShop.Models;
using Core6_EShop.ViewModel;

namespace Core6_EShop.Dto
{
    public class GoodsDetailDto
    {
        public Cart cartData { get; set; }
        public GoodsViewModel goodsData { get; set; }
    }
}
