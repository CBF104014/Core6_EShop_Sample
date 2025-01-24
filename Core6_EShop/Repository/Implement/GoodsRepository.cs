using Core6_EShop.Cls;
using Core6_EShop.Dto;
using Core6_EShop.Models;
using Core6_EShop.Repository.Base;
using Core6_EShop.ViewModel;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Data;

namespace Core6_EShop.Repository.Implement
{
    public class GoodsRepository : BaseRepository<Goods>
    {
        private GoodsRelationRepository goodsRelationRepository { get; set; }
        private GoodsSizeRepository goodsSizeRepository { get; set; }
        public GoodsRepository(GoodsRelationRepository goodsRelationRepository, GoodsSizeRepository goodsSizeRepository)
        {
            this.goodsRelationRepository = goodsRelationRepository;
            this.goodsSizeRepository = goodsSizeRepository;
        }
        /// <summary>
        /// 商品Grid
        /// </summary>
        public DataTablesResultDto GetGoodsGrid(DataTablesDto dataTablesDto)
        {
            return GetBaseGrid(dataTablesDto, SelAll<GoodsViewModel>());
        }
        /// <summary>
        /// 取得新的商品編號
        /// </summary>
        public async Task<int> GetNewGId(IDbConnection conn = null, IDbTransaction tran = null)
        {
            //TODO: Year要改
            var sqlCmd = $"select ifnull(max(gId),0) gId from {Code.tableCode.Goods.TableFullName}";
            var newGId = await QueryFirstAsync<int>(sqlCmd, conn: conn, tran: tran);
            if (newGId == 0)
            {
                newGId = DateTime.Now.Year * 100000;
            }
            return newGId + 1;
        }
        /// <summary>
        /// 查詢單一商品
        /// </summary>
        public async Task<GoodsFileViewModel> SelByGId(int gId, IWebHostEnvironment env)
        {
            
            var goodsTask = SelFirst<GoodsFileViewModel>("gId=@gId", new { gId });
            var goodsRelationTask = goodsRelationRepository.SelByGIdToViewModel(gId);
            await Task.WhenAll(goodsTask, goodsRelationTask);
            if (goodsTask.Result == null)
            {
                return GetSampleData();
            }
            else
            {
                goodsTask.Result.basePath = env.WebRootPath;
                goodsTask.Result.goodsSizeDatas = goodsRelationTask.Result.ToList();
                goodsTask.Result.fileData = new FileViewModel()
                {
                    fileName = "main",
                    fileType = "png",
                    byteData = File.ReadAllBytes(goodsTask.Result.fullPath),
                };
                return goodsTask.Result;
            }
        }
        /// <summary>
        /// 查詢多項商品
        /// </summary>
        public async Task<IEnumerable<GoodsViewModel>> SelMultipleByGIdToViewModel(int[] gIdDatas)
        {
            var sqlParam = new Dictionary<string, object>();
            for (int i = 0; i < gIdDatas.Length; i++)
            {
                sqlParam.Add($"gid_{i}", gIdDatas[i]);
            }
            var whereCmd = String.Join(",", sqlParam.Select(x => $"@{x.Key}"));
            var sqlCmd = $"select * from {Code.tableCode.Goods.TableFullName} where gId in ({whereCmd})";
            var goodsRelationTask = goodsRelationRepository.SelMultipleByGIdToViewModel(gIdDatas);
            var goodsTask = QueryAsync<GoodsViewModel>(sqlCmd, sqlParam);
            await Task.WhenAll(goodsRelationTask, goodsTask);
            foreach (var item in goodsTask.Result)
            {
                if (goodsRelationTask.Result != null)
                {
                    item.goodsSizeDatas = goodsRelationTask.Result
                        .Where(x => x.gId == item.gId)
                        .ToList();
                }
            }
            return goodsTask.Result;
        }
        /// <summary>
        /// 取篩選商品
        /// </summary>
        public async Task<ShopViewModel> SelByFilter(ShopDto shopDtoData = null, IDbConnection conn = null, IDbTransaction tran = null)
        {
            if (shopDtoData == null)
                shopDtoData = GetDefaultShopData();
            var sqlCmd = $"select * from {Code.tableCode.Goods.TableFullName} ";
            var whereAndCmdDatas = new List<string>();
            var otherCmdDatas = new List<string>();
            var whereCmd = "";
            //searchKey
            if (!String.IsNullOrEmpty(shopDtoData.searchKey))
            {
                whereCmd += " and (`goodName` like CONCAT('%', @searchKey, '%') or `goodDesc` like CONCAT('%', @searchKey, '%'))";
            }
            //price
            if (shopDtoData.minPrice != 0)
            {
                whereAndCmdDatas.Add("`goodPrice` >= @minPrice");
            }
            if (shopDtoData.maxPrice != 0)
            {
                whereAndCmdDatas.Add("`goodPrice` <= @maxPrice");
            }
            //sort
            if (!String.IsNullOrEmpty(shopDtoData.sortType))
            {
                if (shopDtoData.sortType == Code.shopSortCode.nameAsc.Key)
                {
                    otherCmdDatas.Add("order by `goodName` asc");
                }
                else if (shopDtoData.sortType == Code.shopSortCode.nameDesc.Key)
                {
                    otherCmdDatas.Add("order by `goodName` desc");
                }
                else if (shopDtoData.sortType == Code.shopSortCode.priceAsc.Key)
                {
                    otherCmdDatas.Add("order by `goodPrice` asc");
                }
                else if (shopDtoData.sortType == Code.shopSortCode.priceDesc.Key)
                {
                    otherCmdDatas.Add("order by `goodPrice` desc");
                }
            }
            //page
            if (shopDtoData.perPageNum != 0)
            {
                otherCmdDatas.Add("limit @currentPageIndex,@perPageNum");
            }
            //===
            if (whereAndCmdDatas.Count > 0)
            {
                whereCmd += " and " + String.Join(" and ", whereAndCmdDatas);
            }
            var otherCmd = String.Join(" ", otherCmdDatas);
            var goodsTask = QueryAsync<GoodsViewModel>($"{sqlCmd} where goodState=@goodState {whereCmd} {otherCmd}", shopDtoData);
            var goodsRelationTask = goodsRelationRepository.SelAllToViewModel();
            var allGoodsCountTask = GetCount();
            await Task.WhenAll(goodsTask, goodsRelationTask, allGoodsCountTask);
            foreach (var item in goodsTask.Result)
            {
                item.goodsSizeDatas = goodsRelationTask.Result
                    .Where(x => x.gId == item.gId)
                    .ToList();
            }
            shopDtoData.pageCount = Convert.ToInt32(Math.Ceiling((decimal)allGoodsCountTask.Result / shopDtoData.perPageNum));
            if (shopDtoData.currentPageNum > shopDtoData.pageCount)
                shopDtoData.currentPageNum = shopDtoData.pageCount;
            return new ShopViewModel()
            {
                shopDtoData = shopDtoData,
                goodsDatas = goodsTask.Result,
            };
        }
        /// <summary>
        /// 取預設值
        /// </summary>
        public ShopDto GetDefaultShopData()
        {
            return new ShopDto()
            {
                currentPageNum = 1,
                sortType = Code.shopSortCode.@default.Key,
                perPageNum = 20,
            }.Init();
        }
        /// <summary>
        /// 取預設值
        /// </summary>
        public GoodsFileViewModel GetSampleData()
        {
            var goodsViewModelData = new GoodsFileViewModel()
            {
                goodState = 1,
            }.Init();
            goodsViewModelData.fileData = new FileViewModel()
            {
                fileName = "main",
                fileType = "png",
                //byteData = GetSampleImage()
            };
            return goodsViewModelData;
        }
        /// <summary>
        /// 寫入商品
        /// </summary>
        public async Task<int> AddGoods(IWebHostEnvironment _env, GoodsFileViewModel goodsData, IDbConnection conn = null, IDbTransaction tran = null)
        {
            goodsData.basePath = _env.WebRootPath;
            using (conn = GetConnection(conn))
            {
                conn.Open();
                using (tran = GetTransaction(conn, tran))
                {
                    goodsData.gId = await GetNewGId(conn: conn, tran: tran);
                    await Create(goodsData.Mapping<Goods>(), conn: conn, tran: tran);
                    foreach (var item in goodsData.goodsSizeDatas)
                    {
                        item.gId = goodsData.gId;
                    }
                    await goodsRelationRepository.AddRelation(goodsData.goodsSizeDatas, conn: conn, tran: tran);
                    await AddImage(_env, goodsData);
                    tran.Commit();
                }
            }
            return (int)Code.stateCode.success;
        }
        /// <summary>
        /// 刪除商品
        /// </summary>
        public async Task<int> DelByGId(IWebHostEnvironment _env, int gId, IDbConnection conn = null, IDbTransaction tran = null)
        {
            var goodsData = new GoodsViewModel() { gId = gId };
            using (conn = GetConnection(conn))
            {
                conn.Open();
                using (tran = GetTransaction(conn, tran))
                {
                    await Delete("gId=@gId", new { gId }, conn: conn, tran: tran);
                    await goodsRelationRepository.DelByGId(gId, conn: conn, tran: tran);
                    DelImage(_env, goodsData);
                    tran.Commit();
                }
            }
            return (int)Code.stateCode.success;
        }
        /// <summary>
        /// 更新商品
        /// </summary>
        public async Task<int> UpdGoods(IWebHostEnvironment _env, GoodsFileViewModel goodsData, IDbConnection conn = null, IDbTransaction tran = null)
        {
            using (conn = GetConnection(conn))
            {
                conn.Open();
                using (tran = GetTransaction(conn, tran))
                {
                    await UpdByRankey(goodsData, conn: conn, tran: tran);
                    await goodsRelationRepository.DelByGId(goodsData.gId, conn: conn, tran: tran);
                    await goodsRelationRepository.AddRelation(goodsData.goodsSizeDatas, conn: conn, tran: tran);
                    DelImage(_env, goodsData);
                    await AddImage(_env, goodsData);
                    tran.Commit();
                }
            }
            return (int)Code.stateCode.success;
        }

        #region 圖片處理
        /// <summary>
        /// 寫入圖片
        /// </summary>
        public async Task<bool> AddImage(IWebHostEnvironment _env, GoodsFileViewModel goodsData)
        {
            Directory.CreateDirectory(goodsData.fullPathWithoutImage);
            var task1 = File.WriteAllBytesAsync(goodsData.fullPath, goodsData.fileData.byteData);
            var task2 = File.WriteAllBytesAsync(goodsData.thumbnail_sPath, GetThumbnail(goodsData.fileData.byteData, 100, 100));
            var task3 = File.WriteAllBytesAsync(goodsData.thumbnail_mPath, GetThumbnail(goodsData.fileData.byteData, 400, 400));
            await Task.WhenAll(task1, task2, task3);
            return true;
        }
        /// <summary>
        /// 刪除圖片
        /// </summary>
        public bool DelImage(IWebHostEnvironment _env, GoodsViewModel goodsData)
        {
            var imagePath = Path.Combine(_env.WebRootPath, goodsData.fullPathWithoutImage);
            if (Directory.Exists(imagePath))
                Directory.Delete(imagePath, true);
            return true;
        }
        /// <summary>
        /// 處理縮圖
        /// </summary>
        public byte[] GetThumbnail(byte[] imageData, int width, int height)
        {
            var outputImage = new MemoryStream();
            using (var image = Image.Load(imageData))
            {
                image.Mutate(x => x.Resize(width, height));
                image.Save(outputImage, new PngEncoder());
            }
            return outputImage.ToArray();
        }
        /// <summary>
        /// 產生範例圖片(測試用)
        /// </summary>
        public static byte[] GetSampleImage()
        {
            int width = 600;
            int height = 600;
            using (Image image = new Image<Rgba32>(width, height))
            {
                image.Mutate(x => x.BackgroundColor(Color.LightBlue));
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, new SixLabors.ImageSharp.Formats.Png.PngEncoder());
                    return ms.ToArray();
                }
            }
        }
        #endregion
    }
}
