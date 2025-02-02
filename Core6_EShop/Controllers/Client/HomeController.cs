using Core6_EShop.Cls;
using Core6_EShop.Controllers.Base;
using Core6_EShop.Dto;
using Core6_EShop.Models;
using Core6_EShop.Service.Implement;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

//參考範例
//https://github.com/ritwickdey/Cake-Shop

namespace Core6_EShop.Controllers.Client
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private ArduinoA1Service arduinoA1Service { get; set; }
        public HomeController(ILogger<HomeController> logger, ArduinoA1Service arduinoA1Service)
        {
            this._logger = logger;
            this.arduinoA1Service = arduinoA1Service;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ArduinoControl()
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

        [HttpGet]
        public async Task<ArduinoA1> GetArduinoA1Data(string UserId)
        {
            //Home/GetArduinoA1Data?UserId=goose
            if (String.IsNullOrEmpty(UserId))
                return new ArduinoA1().Init();
            var Data = await arduinoA1Service.SelByUserId(UserId);
            return Data ?? new ArduinoA1().Init();
        }
        [HttpPost]
        public async Task<bool> PutArduinoA1Data([FromBody] ArduinoA1 putData)
        {
            /*
             $.ajax({
                type: "POST",
                url: 'https://localhost:7058/Home/Put',
                data: JSON.stringify({
                  "rankey": 1,
                  "userId": "goose",
                  "port4": true,
                  "port5": true,
                  "servo1SpeedL": 4,
                  "servo1SpeedR": 4
                }),
                contentType: "application/json",
                success: function (rs) {
                    console.log(rs);
                },
                error: function (rs) {
                    console.log(rs);
                }
            });
             */
            var Data = arduinoA1Service.SelByUserId(putData.UserId);
            if (Data != null)
                await arduinoA1Service.UpdByRankey(putData);
            return Data != null;
        }
    }
}