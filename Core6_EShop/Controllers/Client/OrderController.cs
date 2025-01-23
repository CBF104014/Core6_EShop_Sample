using Core6_EShop.Cls;
using Core6_EShop.Controllers.Base;
using Core6_EShop.Dto;
using Core6_EShop.Service.Implement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core6_EShop.Controllers.Client
{
    public class OrderController : BaseController
    {
        private readonly IWebHostEnvironment env;
        private OrderService orderService;
        private OrderRelationService orderRelationService;
        private CartService cartService;
        public OrderController(IWebHostEnvironment env, OrderService orderService, CartService cartService, OrderRelationService orderRelationService)
        {
            this.env = env;
            this.orderService = orderService;
            this.cartService = cartService;
            this.orderRelationService = orderRelationService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetOrderGrid([FromBody] DataTablesDto dataTablesDto)
        {
            var memberId = CheckToken(User.Claims);
            if (memberId == 0)
                return Json(new APIDto((int)Code.stateCode.error, "Token解析錯誤", ""));
            var gridResult = await orderService.GetOrderGrid(memberId, dataTablesDto);
            return Json(gridResult);
        }
        /// <summary>
        /// 購物車已勾選資料
        /// </summary>
        [Authorize]
        public async Task<IActionResult> GetPreOrderData()
        {
            var memberId = CheckToken(User.Claims);
            if (memberId == 0)
                return Json(new APIDto((int)Code.stateCode.error, "Token解析錯誤", ""));
            var cartDatas = await cartService.SelPreOrderById(memberId);
            var orderData = orderService.GetSampleData(memberId);
            return Json(new APIDto((int)Code.stateCode.success, "", "", new
            {
                cartDatas,
                orderData,
                gType1CodeDatas = Code.gType1Code.selfDatas,
            }));
        }
        [Authorize]
        public async Task<IActionResult> GetOrderData()
        {
            var memberId = CheckToken(User.Claims);
            if (memberId == 0)
                return Json(new APIDto((int)Code.stateCode.error, "Token解析錯誤", ""));
            var orderDatas = await orderService.SelByMemberIdToViewModel(memberId);
            return Json(new APIDto((int)Code.stateCode.success, "", "", new
            {
                orderDatas
            }));
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetOrderRelationData(long orderId)
        {
            var memberId = CheckToken(User.Claims);
            if (memberId == 0)
                return Json(new APIDto((int)Code.stateCode.error, "Token解析錯誤", ""));
            var orderRelationDatas = await orderRelationService.SelByOrderIdToViewModel(orderId, env);
            return Json(new APIDto((int)Code.stateCode.success, "", "", new
            {
                orderRelationDatas
            }));
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveOrderData([FromBody] OrderDto orderDtoData)
        {
            var memberId = CheckToken(User.Claims);
            if (memberId == 0)
                return Json(new APIDto((int)Code.stateCode.error, "Token解析錯誤", ""));
            var validateData = ValidateModel();
            if (!validateData.isValid)
            {
                return Json(new APIDto((int)Code.stateCode.error, $"資料未填寫完整", "", new
                {
                    validateData.validateDatas
                }));
            }
            var orderResult = await orderService.SaveData(memberId, orderDtoData);
            return Json(orderResult);
        }
    }
}
