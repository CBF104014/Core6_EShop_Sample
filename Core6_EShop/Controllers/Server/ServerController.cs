using Core6_EShop.Cls;
using Core6_EShop.Controllers.Base;
using Core6_EShop.Dto;
using Core6_EShop.Models;
using Core6_EShop.Service.Implement;
using Core6_EShop.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Core6_EShop.Controllers.Server
{
    [Authorize(Roles = "admin")]
    public class ServerController : BaseController
    {
        private readonly IWebHostEnvironment env;
        private GoodsService goodsService { get; set; }
        private GoodsRelationService goodsRelationService { get; set; }
        private GoodsSizeService goodsSizeService { get; set; }
        private string goodsPath { get => $"{Path.Combine(env.WebRootPath, "image", "goods")}"; }
        private string imageOtherPath { get => $"{Path.Combine(env.WebRootPath, "image", "other")}"; }
        public ServerController(IWebHostEnvironment env, GoodsService goodsService, GoodsRelationService goodsRelationService, GoodsSizeService goodsSizeService)
        {

            this.env = env;
            this.goodsService = goodsService;
            this.goodsRelationService = goodsRelationService;
            this.goodsSizeService = goodsSizeService;
        }
        #region View
        public IActionResult GoodsManage()
        {
            return View();
        }
        public IActionResult CodeManage()
        {
            return View();
        }
        #endregion

        #region Grid
        [HttpPost]
        public IActionResult GetGoodsGrid([FromBody] DataTablesDto dataTablesDto)
        {
            return Json(goodsService.GetGoodsGrid(dataTablesDto));
        }
        [HttpPost]
        public async Task<IActionResult> GetGoodsRelationGrid([FromBody] DataTablesDto dataTablesDto, string systemName)
        {
            var dataParameter = dataTablesDto.dataParameter.MappingFromBody<GoodsRelation>();
            var gridResult = await goodsRelationService.GetGoodsRelationGrid(dataTablesDto, dataParameter.gId);
            return Json(gridResult);
        }
        #endregion

        #region Goods
        [HttpPost]
        public async Task<IActionResult> SaveGoods([FromBody] GoodsFileViewModel goodsData)
        {
            var validateData = ValidateModel();
            if (!validateData.isValid)
            {
                return Json(new APIDto((int)Code.stateCode.error, $"資料未填寫完整", "", new
                {
                    validateData.validateDatas
                }));
            }
            await goodsService.SaveGoods(env, goodsData);
            return Json(new APIDto((int)Code.stateCode.success, "存檔成功", "", new { }));
        }
        [HttpPost]
        public async Task<IActionResult> GetAllGoodsData()
        {
            var allGoodsDatas = await goodsService
                .SelAllAsync<GoodsViewModel>();
            return Json(new APIDto((int)Code.stateCode.success, "", "", new
            {
                allGoodsDatas
            }));
        }
        [HttpGet]
        public async Task<IActionResult> DeleteGoods(int gId)
        {
            await goodsService.DelByGId(env, gId);
            return Json(new APIDto((int)Code.stateCode.success, $"編號「{gId}」刪除成功", "", new
            {
            }));
        }
        [HttpPost]
        public IActionResult UploadOther([FromBody] List<FileViewModel> fileDatas)
        {
            var successCnt = 0;
            var errorCnt = 0;
            foreach (var item in fileDatas)
            {
                try
                {
                    Directory.CreateDirectory(imageOtherPath);
                    System.IO.File.WriteAllBytes(Path.Combine(imageOtherPath, $"{item.fileName}.{item.fileType}"), item.byteData);
                    successCnt++;
                }
                catch (Exception)
                {
                    errorCnt++;
                }
            }
            return Json(new APIDto((int)Code.stateCode.success, $"新增成功{successCnt}筆，失敗{errorCnt}筆", "", new
            {
            }));
        }
        [HttpPost]
        public IActionResult GetAllOtherFile()
        {
            var imagePath = new DirectoryInfo(imageOtherPath);
            var imageFiles = imagePath.GetFiles("*.jpg")
                .Concat(imagePath.GetFiles("*.png"))
                .Select(x => new ImageOtherDto
                {
                    name = x.Name,
                });
            return Json(new APIDto((int)Code.stateCode.success, "", "", new
            {
                imageFiles
            }));
        }
        [HttpPost]
        public IActionResult DeleteImageOther([FromBody] ImageOtherDto imageData)
        {
            var imagePath = Path.Combine(imageOtherPath, imageData.name);
            System.IO.File.Delete(imagePath);
            return Json(new APIDto((int)Code.stateCode.success, $"「{imageData.name}」刪除成功", "", new
            {
            }));
        }
        #endregion

        #region GoodsRelation
        /// <summary>
        /// 新增關聯
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SaveGoodsRelation([FromBody] GoodsRelationViewModel goodsRelationData)
        {
            var validateData = ValidateModel();
            if (!validateData.isValid)
            {
                return Json(new APIDto((int)Code.stateCode.error, $"資料未填寫完整", "", new
                {
                    validateData.validateDatas
                }));
            }
            await goodsRelationService.SaveRelation(goodsRelationData);
            return Json(new APIDto((int)Code.stateCode.success, $"存檔成功", "", new { }));
        }
        /// <summary>
        /// 刪除關聯
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DelGoodsRelation(int gId, int sizeId)
        {
            await goodsRelationService.DelBySizeId(gId, sizeId);
            return Json(new APIDto((int)Code.stateCode.success, $"刪除成功", "", new { }));
        }
        #endregion

        #region Modal視窗
        /// <summary>
        /// 開啟商品明細管理視窗
        /// </summary>
        public async Task<IActionResult> LoadGoodsDetailManageModal(int gId)
        {
            var goodsDetailManageData = new GoodsDetailManageDto();
            goodsDetailManageData.goodsViewModelData = await goodsService.SelByGId(gId, env);
            goodsDetailManageData.goodsSizeCodeDatas = await goodsSizeService.SelAllAsync<GoodsSize>();
            goodsDetailManageData.gType1CodeDatas = Code.gType1Code.selfDatas;
            goodsDetailManageData.goodStateCodeDatas = Code.goodStateCode.selfDatas;
            return PartialView("GoodsDetailManage", goodsDetailManageData);
        }
        /// <summary>
        /// 開啟尺寸管理視窗
        /// </summary>
        public async Task<IActionResult> LoadGoodsRelationManageModal(int gId, int sizeId)
        {
            var goodsRelationManageData = new GoodsRelationManageDto();
            goodsRelationManageData.goodsSizeDatas = await goodsSizeService.SelAllAsync<GoodsSize>();
            if (sizeId == 0)
            {
                goodsRelationManageData.goodsRelationData = new GoodsRelationViewModel() { gId = gId }.Init();
            }
            else
            {
                goodsRelationManageData.goodsRelationData = await goodsRelationService.SelBySizeIdToViewModel(gId, sizeId);
            }
            return PartialView("GoodsRelationManage", goodsRelationManageData);
        }
        #endregion














        //======================================================
        //以下為測試
        //======================================================

        /// <summary>
        /// 批次新增測試
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> BatchTest()
        {
            var data = goodsService.GetSampleData();
            data.goodName = "測試商品";
            data.goodDesc = "描述描述描述描述描述描述";
            data.gType1 = Code.gType1Code.noodle.varValue;
            var tasksLen = 3;
            var sTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
            var tasks = new Task[tasksLen];
            for (int i = 0; i < tasksLen; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    data.goodPrice = 10 + new Random().Next(((300 - 10) / 5) + 1) * 5;
                    await goodsService.AddGoods(env, data.CloneObj());
                });
            }
            await Task.WhenAll(tasks);
            var eTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
            return Json(new APIDto((int)Code.stateCode.success, $"成功", "", new
            {
                sTime,
                eTime,
            }));
        }
        //[HttpPost]
        //public IActionResult UploadVideo([FromBody] GoodsViewModel goodsData)
        //{
        //    if (String.IsNullOrEmpty(goodsData.fileData.fileName))
        //        goodsData.fileData.fileName = DateTime.Now.ToString("yyyyMMddTHHmmss");
        //    Directory.CreateDirectory(Path.Combine(env.WebRootPath, "video"));
        //    System.IO.File.WriteAllBytes(Path.Combine(env.WebRootPath, "video", $"{goodsData.fileData.fileName}.{goodsData.fileData.fileType}"), goodsData.fileData.byteData);
        //    return Json(new APIDto((int)Code.stateCode.success, "新增成功", "", new
        //    {
        //    }));
        //}
        [HttpPost]
        public IActionResult GetAllVideo()
        {
            var videoPath = new DirectoryInfo(Path.Combine(env.WebRootPath, "video"));
            var viedoFiles = videoPath.GetFiles("*.mp4")
                .Select(x => new VideoDto
                {
                    name = x.Name,
                });
            return Json(new APIDto((int)Code.stateCode.success, "", "", new
            {
                viedoFiles
            }));
        }
        [HttpPost]
        public IActionResult DeleteVideo([FromBody] VideoDto videoData)
        {
            var path = env.WebRootPath;
            videoData.videoSrc.Split('/').ToList().ForEach(item =>
            {
                if (!String.IsNullOrEmpty(item))
                    path = Path.Combine(path, item);
            });
            System.IO.File.Delete(path);
            return Json(new APIDto((int)Code.stateCode.success, "刪除成功", "", new
            {
            }));
        }
    }
}
