using Core6_EShop.Cls;
using Core6_EShop.Dto;
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
        /// <summary>
        /// 取預設值
        /// </summary>
        public ShopDto GetDefaultShopData()
        {
            return new Dto.ShopDto()
            {
                currentPageNum = 1,
                sortType = 0,
                perPageNum = 20,
            }.Init();
        }
        /// <summary>
        /// 篩選後商品資料
        /// </summary>
        public ShopViewModel SelByFilter(ShopDto shopDtoData)
        {
            if (shopDtoData == null)
                shopDtoData = GetDefaultShopData();
            var goodsDatas = SelAll<GoodsViewModel>((shopDtoData.currentPageNum - 1) * shopDtoData.perPageNum, shopDtoData.perPageNum);
            var allGoodsCount = GetCount();
            shopDtoData.pageCount = Convert.ToInt32(Math.Ceiling((decimal)allGoodsCount / shopDtoData.perPageNum));
            if (shopDtoData.currentPageNum > shopDtoData.pageCount)
                shopDtoData.currentPageNum = shopDtoData.pageCount;
            return new ShopViewModel()
            {
                ShopDtoData = shopDtoData,
                GoodsDatas = goodsDatas,
            };
        }
        /// <summary>
        /// 寫入商品
        /// </summary>
        public void CreateGoods(IWebHostEnvironment _webHostEnvironment, GoodsViewModel goodsData)
        {
            goodsData.basePath = _webHostEnvironment.WebRootPath;
            Directory.CreateDirectory(goodsData.fullPathWithoutImage);
            File.WriteAllBytes(goodsData.fullPath, goodsData.fileData.byteData);
            Add(goodsData.Mapping<Goods>());
        }
    }
}
