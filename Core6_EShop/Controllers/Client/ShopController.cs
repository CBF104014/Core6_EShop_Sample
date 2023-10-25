using Core6_EShop.Dto;
using Core6_EShop.Service.Implement;
using Core6_EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;
using static Core6_EShop.Cls.Code;

namespace Core6_EShop.Controllers.Client
{
    public class ShopController : Controller
    {
        private GoodsService _goodsService { get; set; }
        public ShopController(GoodsService goodsService)
        {
            this._goodsService = goodsService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetData([FromBody] ShopDto shopDtoData)
        {
            var shopViewModelData = _goodsService.GetDefaultShopData();
            var allGoodsCount = _goodsService.GetCount();
            if (shopDtoData != null)
                shopViewModelData.ShopDtoData = shopDtoData;
            var perPageNum = shopViewModelData.ShopDtoData.perPageNum;
            var currentPageNum = shopViewModelData.ShopDtoData.currentPageNum;
            shopViewModelData.GoodsDatas = _goodsService.SelAll((currentPageNum - 1) * perPageNum, perPageNum);
            shopViewModelData.ShopDtoData.pageCount = Convert.ToInt32(Math.Ceiling((decimal)allGoodsCount / perPageNum));
            if (currentPageNum > shopViewModelData.ShopDtoData.pageCount)
                shopViewModelData.ShopDtoData.currentPageNum = shopViewModelData.ShopDtoData.pageCount;
            //shopViewModelData.ShopDtoDat.
            return Json(new APIDto((int)stateCode.success, "", "", new
            {
                shopViewModelData
            }));
        }
    }
}
