using Core6_EShop.Dto;
using Core6_EShop.Models;
using Core6_EShop.Service.Implement;
using Core6_EShop.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using static Core6_EShop.Cls.Code;

namespace Core6_EShop.Controllers.Server
{
    public class ServerController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private GoodsService _goodsService { get; set; }
        public ServerController(IWebHostEnvironment env, GoodsService goodsService) {

            this._env = env;
            this._goodsService = goodsService;
        }
        public IActionResult GoodsManage()
        {
            return View();
        }
        public IActionResult CodeManage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetSampleData()
        {
            var goodsViewModelData = new GoodsViewModel().Init();
            if (goodsViewModelData.GId == 0)
                goodsViewModelData.GId = 1;
            return Json(new APIDto((int)stateCode.success, "", "", new
            {
                goodsViewModelData
            }));
        }
        [HttpPost]
        public IActionResult UploadImg([FromBody] GoodsViewModel goodsData)
        {
            _goodsService.CreateGoods(_env, goodsData);
            return Json(new APIDto((int)stateCode.success, "新增成功", "", new
            {
            }));
        }
        [HttpPost]
        public IActionResult UploadVideo([FromBody] GoodsViewModel goodsData)
        {
            if (String.IsNullOrEmpty(goodsData.fileData.fileName))
                goodsData.fileData.fileName = DateTime.Now.ToString("yyyyMMddTHHmmss");
            Directory.CreateDirectory(Path.Combine(_env.WebRootPath, "video"));
            System.IO.File.WriteAllBytes(Path.Combine(_env.WebRootPath, "video", $"{goodsData.fileData.fileName}.{goodsData.fileData.fileType}"), goodsData.fileData.byteData);
            return Json(new APIDto((int)stateCode.success, "新增成功", "", new
            {
            }));
        }
        //測試
        [HttpPost]
        public IActionResult GetAllVideo()
        {
            var videoPath = new DirectoryInfo(Path.Combine(_env.WebRootPath, "video"));
            var viedoFiles = videoPath.GetFiles("*.mp4")
                .Select(x => new VideoDto
                {
                    name = x.Name,
                });
            return Json(new APIDto((int)stateCode.success, "", "", new
            {
                viedoFiles
            }));
        }
        [HttpPost]
        public IActionResult DeleteVideo([FromBody] VideoDto videoData)
        {
            var path = _env.WebRootPath;
            videoData.videoSrc.Split('/').ToList().ForEach(item =>
            {
                if (!String.IsNullOrEmpty(item))
                    path = Path.Combine(path, item);
            });
            System.IO.File.Delete(path);
            return Json(new APIDto((int)stateCode.success, "刪除成功", "", new
            {
            }));
        }
    }
}
