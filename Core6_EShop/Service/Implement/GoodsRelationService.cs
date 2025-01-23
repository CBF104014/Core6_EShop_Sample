using Core6_EShop.Cls;
using Core6_EShop.Dto;
using Core6_EShop.Models;
using Core6_EShop.Repository.Implement;
using Core6_EShop.Service.Base;
using Core6_EShop.ViewModel;
using System.Data;
using System.Threading.Tasks;

namespace Core6_EShop.Service.Implement
{
    public class GoodsRelationService : BaseService<GoodsRelation>
    {
        private GoodsRelationRepository goodsRelationRepository { get; set; }
        public GoodsRelationService(GoodsRelationRepository goodsRelationRepository) : base(goodsRelationRepository)
        {
            this.goodsRelationRepository = goodsRelationRepository;
        }
        public Task<DataTablesResultDto> GetGoodsRelationGrid(DataTablesDto dataTablesDto, int gId)
        {
            return goodsRelationRepository.GetGoodsRelationGrid(dataTablesDto, gId);
        }
        public async Task<IEnumerable<GoodsRelationViewModel>> SelAllToViewModel()
        {
            return await goodsRelationRepository.SelAllToViewModel();
        }
        public async Task<IEnumerable<GoodsRelationViewModel>> SelByGIdToViewModel(int gId)
        {
            return await goodsRelationRepository.SelByGIdToViewModel(gId);
        }
        public async Task<GoodsRelationViewModel> SelBySizeIdToViewModel(int gId, int sizeId)
        {
            return await goodsRelationRepository.SelBySizeIdToViewModel(gId, sizeId);
        }
        public async Task<int> AddRelation(GoodsRelationViewModel goodsRelationData, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await goodsRelationRepository.Create(goodsRelationData, conn: conn, tran: tran);
        }
        public async Task<int> AddRelation(IEnumerable<GoodsRelationViewModel> goodsRelationDatas, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await goodsRelationRepository.AddRelation(goodsRelationDatas, conn: conn, tran: tran);
        }
        public async Task<int> DelBySizeId(int gId, int sizeId, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await goodsRelationRepository.DelBySizeId(gId, sizeId, conn: conn, tran: tran);
        }
        public async Task<int> SaveRelation(GoodsRelationViewModel goodsRelationData, IDbConnection conn = null, IDbTransaction tran = null)
        {
            if (goodsRelationData.rankey == 0)
            {
                return await goodsRelationRepository.Create(goodsRelationData, conn: conn, tran: tran);
            }
            else
            {
                return await goodsRelationRepository.UpdByRankey(goodsRelationData, conn: conn, tran: tran);
            }
        }
    }
}
