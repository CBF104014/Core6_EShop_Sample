using Core6_EShop.Cls;
using Core6_EShop.Models;
using Core6_EShop.Service.Base;
using Core6_EShop.ViewModel;

namespace Core6_EShop.Service.Implement
{
    public class GoodsService : BaseService<Goods>
    {
        public Goods SelByPK(string gType1, string gType2, int gId)
        {
            return _mySqlTool.Read("GType1=@GType1 and GType2=@GType2 and GId=@GId", new Dictionary<string, object>()
            {
                ["GType1"] = gType1,
                ["GType2"] = gType2,
                ["GId"] = gId,
            }).FirstOrDefault();
        }
        public int Add(Goods goodsData)
        {
            return _mySqlTool.Create(goodsData);
        }

        public ShopViewModel GetDefaultShopData()
        {
            return new ShopViewModel()
            {
                ShopDtoData = new Dto.ShopDto()
                {
                    currentPageNum = 1,
                    sortType = 0,
                    perPageNum = 20,
                }.Init(),
                GoodsDatas = new List<Goods>(),
            };
        }
    }
}
