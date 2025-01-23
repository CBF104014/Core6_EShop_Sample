using Core6_EShop.Cls;
using Core6_EShop.Dto;
using Core6_EShop.Models;
using Core6_EShop.Repository.Implement;
using Core6_EShop.Service.Base;
using Core6_EShop.ViewModel;
using System.Data;

namespace Core6_EShop.Service.Implement
{
    public class GoodsService : BaseService<Goods>
    {
        private GoodsRepository goodsRepository { get; set; }
        public GoodsService(GoodsRepository goodsRepository) : base(goodsRepository)
        {
            this.goodsRepository = goodsRepository;
        }
        public DataTablesResultDto GetGoodsGrid(DataTablesDto dataTablesDto)
        {
            return goodsRepository.GetGoodsGrid(dataTablesDto);
        }
        /// <summary>
        /// 取得新的商品編號
        /// </summary>
        public async Task<int> GetNewGId()
        {
            return await goodsRepository.GetNewGId();
        }
        /// <summary>
        /// 篩選後商品資料
        /// </summary>
        public async Task<ShopViewModel> SelByFilter(ShopDto shopDtoData = null, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await goodsRepository.SelByFilter(shopDtoData, conn: conn, tran: tran);
        }
        /// <summary>
        /// 查詢單一商品
        /// </summary>
        public async Task<GoodsFileViewModel> SelByGId(int gId, IWebHostEnvironment env)
        {
            return await goodsRepository.SelByGId(gId, env);
        }
        /// <summary>
        /// 取預設值
        /// </summary>
        public ShopDto GetDefaultShopData()
        {
            return goodsRepository.GetDefaultShopData();
        }
        /// <summary>
        /// 取商品範本
        /// </summary>
        public GoodsFileViewModel GetSampleData()
        {
            return goodsRepository.GetSampleData();
        }
        /// <summary>
        /// 商品存檔
        /// </summary>
        public async Task<int> SaveGoods(IWebHostEnvironment _env, GoodsFileViewModel goodsData, IDbConnection conn = null, IDbTransaction tran = null)
        {
            if (goodsData.rankey > 0)
                return await goodsRepository.UpdGoods(_env, goodsData, conn: conn, tran: tran);
            else
                return await goodsRepository.AddGoods(_env, goodsData, conn: conn, tran: tran);
        }
        /// <summary>
        /// 寫入商品與圖片
        /// </summary>
        public async Task<int> AddGoods(IWebHostEnvironment _env, GoodsFileViewModel goodsData, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await goodsRepository.AddGoods(_env, goodsData, conn: conn, tran: tran);
        }
        /// <summary>
        /// 刪除商品與圖片
        /// </summary>
        public async Task<int> DelByGId(IWebHostEnvironment _env, int gId, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await goodsRepository.DelByGId(_env, gId, conn: conn, tran: tran);
        }
        /// <summary>
        /// 更新商品與圖片
        /// </summary>
        public async Task<int> UpdGoods(IWebHostEnvironment _env, GoodsFileViewModel goodsData, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await goodsRepository.UpdGoods(_env, goodsData, conn: conn, tran: tran);
        }
    }
}
