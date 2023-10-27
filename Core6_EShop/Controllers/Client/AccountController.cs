using Core6_EShop.Dto;
using Core6_EShop.Service.Implement;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Core6_EShop.Cls.Code;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Core6_EShop.Cls;
using Microsoft.AspNetCore.Authorization;

namespace Core6_EShop.Controllers.Client
{
    public class AccountController : Controller
    {
        private MemberService _memberService { get; set; }
        private readonly IConfiguration _configuration;
        public AccountController(MemberService memberService, IConfiguration configuration)
        {
            _memberService = memberService;
            _configuration = configuration;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult UserInfo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CheckUser([FromBody] LoginDto LoginDtoData)
        {
            var userData = _memberService.SelById(LoginDtoData.Id);
            if (userData == null)
                return Json(new APIDto((int)stateCode.error, "帳號或密碼錯誤", ""));
            var token = new Helper().CreateToken(_configuration, LoginDtoData.Id);
            return Json(new APIDto((int)stateCode.success, "登入成功", "", new
            {
                token,
                id = userData.Id,
                name = userData.Name,
            }));
        }
        [Authorize]
        [HttpPost]
        public IActionResult GetUserData()
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if(userIdClaim == null || String.IsNullOrEmpty(userIdClaim.Value))
                return Json(new APIDto((int)stateCode.error, "Token解析錯誤", ""));
            var userData = _memberService.SelById(userIdClaim.Value);
            if (userData == null)
                return Json(new APIDto((int)stateCode.error, "查無此帳號", ""));
            return Json(new APIDto((int)stateCode.success, "", "", new
            {
                userData
            }));
        }
    }
}
