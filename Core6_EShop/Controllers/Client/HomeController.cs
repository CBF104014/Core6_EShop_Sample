using Core6_EShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

//參考範例
//https://github.com/ritwickdey/Cake-Shop

namespace Core6_EShop.Controllers.Client
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}