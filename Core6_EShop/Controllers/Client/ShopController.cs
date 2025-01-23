using Core6_EShop.Cls;
using Core6_EShop.Controllers.Base;
using Core6_EShop.Dto;
using Core6_EShop.Models;
using Core6_EShop.Service.Implement;
using Microsoft.AspNetCore.Mvc;

namespace Core6_EShop.Controllers.Client
{
    public class ShopController : BaseController
    {
        private readonly IWebHostEnvironment env;
        private GoodsService goodsService { get; set; }
        private CartService cartService { get; set; }
        public ShopController(IWebHostEnvironment env, GoodsService goodsService, CartService cartService)
        {
            this.env = env;
            this.goodsService = goodsService;
            this.cartService = cartService;
        }
        public async Task<IActionResult> Index()
        {
            var shopViewModelData = await goodsService.SelByFilter();
            return View(shopViewModelData);
        }
        public IActionResult GoodsDetail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetShopData([FromBody] ShopDto shopDtoData)
        {
            var shopViewModelData = await goodsService.SelByFilter(shopDtoData);
            return Json(new APIDto((int)Code.stateCode.success, "", "", new
            {
                shopViewModelData,
            }));
        }
        [HttpGet]
        public async Task<IActionResult> GetGoodsData(int gId)
        {
            var goodsData = await goodsService.SelByGId(gId, env);
            return Json(new APIDto((int)Code.stateCode.success, "", "", new
            {
                goodsData
            }));
        }
        [HttpPost]
        public async Task<IActionResult> GetGoodsDetailData([FromBody] Goods goodsDtoData)
        {
            var cartData = cartService.GetSampleData(0, goodsDtoData.gId, 0, 1);
            var goodsData = await goodsService.SelByGId(goodsDtoData.gId, env);
            return Json(new APIDto((int)Code.stateCode.success, "", "", new
            {
                cartData,
                goodsData,
            }));
        }
        public async Task<IActionResult> LoadGoodsDetailModal(int gId)
        {
            var goodsDetaiulData = new GoodsDetailDto();
            goodsDetaiulData.cartData = cartService.GetSampleData(0, gId, 0, 1);
            goodsDetaiulData.goodsData = await goodsService.SelByGId(gId, env);
            return PartialView("GoodsDetail", goodsDetaiulData);
        }
    }
}
