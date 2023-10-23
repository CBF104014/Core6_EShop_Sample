using Core6_EShop.Dto;
using Core6_EShop.Service.Implement;
using Microsoft.AspNetCore.Mvc;

namespace Core6_EShop.Controllers.Client
{
    public class AccountController : Controller
    {
        private MemberService _memberService { get; set; }
        public AccountController(MemberService memberService)
        {
            _memberService = memberService;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CheckUser([FromBody] LoginDto LoginDtoData)
        {
            var UserData = _memberService.SelById(LoginDtoData.Id);
            return Json(new
            {
                Success = UserData != null,
                UserData
            });
        }
    }
}
