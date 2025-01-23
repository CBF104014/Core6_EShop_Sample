using Core6_EShop.Cls;
using Core6_EShop.Models;
using Core6_EShop.ViewModel;

namespace Core6_EShop.Dto
{
    public class GoodsDetailManageDto
    {
        public GoodsFileViewModel goodsViewModelData { get; set; }
        public IEnumerable<Setting> gType1CodeDatas { get; set; }
        public IEnumerable<Setting> goodStateCodeDatas { get; set; }
        public IEnumerable<GoodsSize> goodsSizeCodeDatas { get; set; }
        public GoodsRelationViewModel goodsRelationSample
        {
            get => new GoodsRelationViewModel()
            {
                gId = this.goodsViewModelData == null ? 0 : this.goodsViewModelData.gId
            }.Init();
        }
    }
}
