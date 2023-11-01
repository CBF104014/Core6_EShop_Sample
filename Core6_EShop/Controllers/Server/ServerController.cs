using Microsoft.AspNetCore.Mvc;

namespace Core6_EShop.Controllers.Server
{
    public class ServerController : Controller
    {
        public IActionResult GoodsManage()
        {
            return View();
        }
    }
}
