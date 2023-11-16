using Core6_EShop.Dto;
using Core6_EShop.Models;
using Core6_EShop.Service.Implement;
using Core6_EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;
using static Core6_EShop.Cls.Code;

namespace Core6_EShop.Controllers.Server
{
    public class ServerController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private GoodsService _goodsService { get; set; }
        public ServerController(IWebHostEnvironment env, GoodsService goodsService) {

            this._env = env;
            this._goodsService = goodsService;
        }
        public IActionResult GoodsManage()
        {
            return View();
        }
        public IActionResult CodeManage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetSampleData()
        {
            var goodsViewModelData = new GoodsViewModel().Init();
            if (goodsViewModelData.GId == 0)
                goodsViewModelData.GId = 1;
            return Json(new APIDto((int)stateCode.success, "", "", new
            {
                goodsViewModelData
            }));
        }
        [HttpPost]
        public IActionResult UpdateImg([FromBody] GoodsViewModel goodsData)
        {
            _goodsService.CreateGoods(_env, goodsData);
            return Json(new APIDto((int)stateCode.success, "新增成功", "", new
            {
            }));
        }
    }
}
