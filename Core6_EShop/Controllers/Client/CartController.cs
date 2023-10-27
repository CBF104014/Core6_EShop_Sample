using Core6_EShop.Dto;
using Core6_EShop.Service.Implement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Core6_EShop.Cls.Code;

namespace Core6_EShop.Controllers.Client
{
    public class CartController : Controller
    {
        private CartService _cartService { get; set; }
        public CartController(CartService cartService)
        {
            this._cartService = cartService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult GetCart()
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (userIdClaim == null || String.IsNullOrEmpty(userIdClaim.Value))
                return Json(new APIDto((int)stateCode.error, "Token解析錯誤", ""));
            var CartDatas = _cartService.SelById(userIdClaim.Value);
            return Json(new APIDto((int)stateCode.success, "", "", new
            {
                cartDatas = CartDatas
            }));
        }
    }
}
