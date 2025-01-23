using Core6_EShop.Cls;
using Core6_EShop.Controllers.Base;
using Core6_EShop.Dto;
using Core6_EShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

//參考範例
//https://github.com/ritwickdey/Cake-Shop

namespace Core6_EShop.Controllers.Client
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public IActionResult GetCode()
        {
            return Json(new APIDto((int)Code.stateCode.success, "", "", new
            {
                goodStateCode = Code.goodStateCode.selfDatas,
                memberStateCode = Code.memberStateCode.selfDatas,
                orderStateCode = Code.orderStateCode.selfDatas,
                paymentTypeCode = Code.paymentTypeCode.selfDatas,
                deliveryTypeCode = Code.deliveryTypeCode.selfDatas,
            }));
        }
    }
}