using Core6_EShop.Cls;
using Core6_EShop.Controllers.Client;
using Core6_EShop.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Core6_EShop.Controllers.Base
{
    public class BaseController : Controller
    {
        private readonly ILogger<ShopController> _logger;
        public BaseController(ILogger<ShopController> _logger = null)
        {
            this._logger = _logger;
        }
        /// <summary>
        /// 檢查Token
        /// </summary>
        public int CheckToken(IEnumerable<Claim> userClaims)
        {
            var userIdClaim = userClaims.FirstOrDefault(x => x.Type == "memberId");
            if (userIdClaim == null || String.IsNullOrEmpty(userIdClaim.Value))
                return 0;
            return Convert.ToInt32(userIdClaim.Value);
        }
        /// <summary>
        /// 驗證Model
        /// </summary>
        public ValidateDto ValidateModel()
        {
            return new ValidateDto()
            {
                isValid = ModelState.IsValid,
                validateDatas = ModelState.IsValid ? null : ModelState
                    .Where(x => x.Value.Errors.Any())
                    .Select(x => new ValidateDetailDto()
                    {
                        fieldFullName = x.Key,
                        errorDatas = x.Value.Errors.Select(y => y.ErrorMessage)
                    })
            };
        }
    }
}
