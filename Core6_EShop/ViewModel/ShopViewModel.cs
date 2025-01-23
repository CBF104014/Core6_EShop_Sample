
using Core6_EShop.Dto;
using Core6_EShop.Models;
using static Core6_EShop.Cls.Code;

namespace Core6_EShop.ViewModel
{
    public class ShopViewModel
    {
        public ShopDto shopDtoData { get; set; }
        public IEnumerable<GoodsViewModel> goodsDatas { get; set; }
        public List<GType1Dto> gType1CodeDatas { get => gType1Code.enableDatas.Mapping<List<GType1Dto>>(); }
    }
}
