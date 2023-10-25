
using Core6_EShop.Dto;
using Core6_EShop.Models;

namespace Core6_EShop.ViewModel
{
    public class ShopViewModel
    {
        public ShopDto ShopDtoData { get; set; }
        public IEnumerable<Goods> GoodsDatas { get; set; }
    }
}
