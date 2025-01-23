using Core6_EShop.Cls;
using Core6_EShop.Controllers.Base;
using Core6_EShop.Dto;
using Core6_EShop.Models;
using Core6_EShop.Service.Implement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core6_EShop.Controllers.Client
{
    public class CartController : BaseController
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
        public async Task<IActionResult> GetCart()
        {
            var memberId = CheckToken(User.Claims);
            if (memberId == 0)
                return Json(new APIDto((int)Code.stateCode.error, "Token解析錯誤", ""));
            var cartDatas = await _cartService.SelToViewModelById(memberId);
            return Json(new APIDto((int)Code.stateCode.success, "", "", new
            {
                cartDatas
            }));
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] Cart cartDto)
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
            var newCartData = _cartService.GetSampleData(memberId, cartDto.gId, cartDto.sizeId, cartDto.quantity);
            await _cartService.AddToCart(newCartData);
            return Json(new APIDto((int)Code.stateCode.success, "加入購物車成功", "", new
            {
            }));
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdCartChecked(bool isChecked)
        {
            var memberId = CheckToken(User.Claims);
            if (memberId == 0)
                return Json(new APIDto((int)Code.stateCode.error, "Token解析錯誤", ""));
            await _cartService.UpdCartChecked(memberId, isChecked);
            return Json(new APIDto((int)Code.stateCode.success, "更新成功", "", new
            {
            }));
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdCart([FromBody] Cart cartDto)
        {
            var memberId = CheckToken(User.Claims);
            if (memberId == 0)
                return Json(new APIDto((int)Code.stateCode.error, "Token解析錯誤", ""));
            cartDto.memberId = memberId;
            await _cartService.UpdCart(cartDto);
            return Json(new APIDto((int)Code.stateCode.success, "更新成功", "", new
            {
            }));
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DelCart([FromBody] Cart cartDto)
        {
            var memberId = CheckToken(User.Claims);
            if (memberId == 0)
                return Json(new APIDto((int)Code.stateCode.error, "Token解析錯誤", ""));
            cartDto.memberId = memberId;
            await _cartService.DelCart(cartDto.rankey);
            return Json(new APIDto((int)Code.stateCode.success, "刪除成功", "", new
            {
            }));
        }
    }
}
