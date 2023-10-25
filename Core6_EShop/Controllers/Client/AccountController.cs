using Core6_EShop.Dto;
using Core6_EShop.Service.Implement;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Core6_EShop.Cls.Code;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

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
        [HttpPost]
        public IActionResult CheckUser([FromBody] LoginDto LoginDtoData)
        {
            var UserData = _memberService.SelById(LoginDtoData.Id);
            if (UserData == null)
                return Json(new APIDto((int)stateCode.error, "帳號或密碼錯誤", ""));

            string token = CreateToken(LoginDtoData.Id);
            return Json(new APIDto((int)stateCode.success, UserData.Name, UserData.Id, new
            {
                token,
                id = UserData.Id,
                name = UserData.Name,
            }));
        }
        private string CreateToken(string username)
        {
            List<Claim> claims = new()
            {                    
                //list of Claims - we only checking username - more claims can be added.
                new Claim("username", Convert.ToString(username)),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
