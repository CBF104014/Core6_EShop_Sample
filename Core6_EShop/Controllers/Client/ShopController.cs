using Core6_EShop.Dto;
using Core6_EShop.Service.Implement;
using Core6_EShop.ViewModel;
using Microsoft.AspNetCore.Authorization;
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
            var shopViewModelData = _goodsService.SelByFilter(shopDtoData);
            return Json(new APIDto((int)stateCode.success, "", "", new
            {
                shopViewModelData
            }));
        }
    }
}
