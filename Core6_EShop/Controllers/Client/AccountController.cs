using Core6_EShop.Dto;
using Core6_EShop.Service.Implement;
using Microsoft.AspNetCore.Mvc;
using Core6_EShop.Cls;
using Microsoft.AspNetCore.Authorization;
using Core6_EShop.Controllers.Base;
using Core6_EShop.Models;

namespace Core6_EShop.Controllers.Client
{
    public class AccountController : BaseController
    {
        private MemberService memberService { get; set; }
        private CountryService countryService { get; set; }
        public AccountController(MemberService memberService, CountryService countryService)
        {
            this.memberService = memberService;
            this.countryService = countryService;
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
        public async Task<IActionResult> CheckUser([FromBody] LoginDto LoginDtoData)
        {
            var userData = await memberService.SelByEmail(LoginDtoData.email);
            if (userData == null)
                return Json(new APIDto((int)Code.stateCode.error, "帳號或密碼錯誤", ""));
            var token = Code.CreateToken(userData);
            return Json(new APIDto((int)Code.stateCode.success, "登入成功", "", new
            {
                token,
                userData.memberId,
                userData.email,
                userData.name,
            }));
        }
        [HttpPost]
        public async Task<IActionResult> GetRegisterData()
        {
            var memberData = memberService.GetSampleData();
            var countryDatas = await countryService.SelAllAsync<Country>();
            return Json(new APIDto((int)Code.stateCode.success, "", "", new
            {
                memberData,
                countryDatas,
            }));
        }
        [HttpPost]
        public async Task<IActionResult> SaveRegisterData([FromBody] Member memberData)
        {
            var validateData = ValidateModel();
            if (!validateData.isValid)
            {
                return Json(new APIDto((int)Code.stateCode.error, $"資料未填寫完整", "", new
                {
                    validateData.validateDatas
                }));
            }
            var apiResult = await memberService.SaveData(memberData);
            return Json(apiResult);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetUserData()
        {
            var memberId = CheckToken(User.Claims);
            if (memberId == 0)
                return Json(new APIDto((int)Code.stateCode.error, "Token解析錯誤", ""));
            var memberData = await memberService.SelById(memberId);
            if (memberData == null)
                return Json(new APIDto((int)Code.stateCode.error, "查無此帳號", ""));
            var countryDatas = await countryService.SelAllAsync<Country>();
            return Json(new APIDto((int)Code.stateCode.success, "", "", new
            {
                memberData,
                countryDatas,
            }));
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveUserData([FromBody] Member memberData)
        {
            var validateData = ValidateModel();
            if (!validateData.isValid)
            {
                return Json(new APIDto((int)Code.stateCode.error, $"資料未填寫完整", "", new
                {
                    validateData.validateDatas
                }));
            }
            var apiResult = await memberService.SaveData(memberData);
            return Json(apiResult);
        }
    }
}
