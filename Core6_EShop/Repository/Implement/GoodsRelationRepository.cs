using Core6_EShop.Cls;
using Core6_EShop.Dto;
using Core6_EShop.Models;
using Core6_EShop.Repository.Base;
using Core6_EShop.ViewModel;
using System.Data;

namespace Core6_EShop.Repository.Implement
{
    public class GoodsRelationRepository : BaseRepository<GoodsRelation>
    {
        public GoodsRelationRepository() { }
        public string GetViewModelCmd()
        {
            var sqlCmd = $@"select 
                                a.*,b.sizeName 
                            from {Code.tableCode.GoodsRelation.TableFullName} as a
                            left join {Code.tableCode.GoodsSize.TableFullName} as b on a.sizeId=b.sizeId";
            return sqlCmd;
        }
        /// <summary>
        /// 商品關聯Grid
        /// </summary>
        public Task<DataTablesResultDto> GetGoodsRelationGrid(DataTablesDto dataTablesDto, int gId)
        {
            return GetBaseGrid(dataTablesDto, SelByGIdToViewModel(gId));
        }
        public async Task<IEnumerable<GoodsRelationViewModel>> SelAllToViewModel()
        {
            var sqlCmd = GetViewModelCmd();
            return await QueryAsync<GoodsRelationViewModel>(sqlCmd);
        }
        public async Task<IEnumerable<GoodsRelationViewModel>> SelByGIdToViewModel(int gId)
        {
            var sqlCmd = $"{GetViewModelCmd()} where a.gId=@gId";
            return await QueryAsync<GoodsRelationViewModel>(sqlCmd, new { gId });
        }
        public async Task<IEnumerable<GoodsRelationViewModel>> SelMultipleByGIdToViewModel(int[] gIdDatas)
        {
            if (gIdDatas.Length == 0)
                return null;
            var sqlParam = new Dictionary<string, object>();
            for (int i = 0; i < gIdDatas.Length; i++)
            {
                sqlParam.Add($"gid_{i}", gIdDatas[i]);
            }
            var whereCmd = String.Join(",", sqlParam.Select(x => $"@{x.Key}"));
            var sqlCmd = $"{GetViewModelCmd()} where a.gId in ({whereCmd})";
            return await QueryAsync<GoodsRelationViewModel>(sqlCmd, sqlParam);
        }
        public async Task<IEnumerable<GoodsRelationViewModel>> SelMultipleBySizeIdToViewModel(int[] gIdDatas, int[] sizeIdDatas)
        {
            if (gIdDatas.Length == 0)
                return null;
            var sqlParam = new Dictionary<string, object>();
            var whereCmdDatas = new List<string>();
            for (int i = 0; i < gIdDatas.Length; i++)
            {
                sqlParam.Add($"gId_{i}", gIdDatas[i]);
                sqlParam.Add($"sizeId_{i}", sizeIdDatas[i]);
                whereCmdDatas.Add($"(a.gId=@gId_{i} and a.sizeId=@sizeId_{i})");
            }
            var sqlCmd = $"{GetViewModelCmd()} where {String.Join(" or ", whereCmdDatas)}";
            return await QueryAsync<GoodsRelationViewModel>(sqlCmd, sqlParam);
        }
        public async Task<GoodsRelationViewModel> SelBySizeIdToViewModel(int gId, int sizeId)
        {
            var sqlCmd = $"{GetViewModelCmd()} where a.gId=@gId and a.sizeId=@sizeId";
            return await QueryFirstAsync<GoodsRelationViewModel>(sqlCmd, new { gId, sizeId });
        }
        public async Task<int> DelByGId(int gId, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await Delete("gId=@gId", new { gId }, conn: conn, tran: tran);
        }
        public async Task<int> DelBySizeId(int gId, int sizeId, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await Delete("gId=@gId and sizeId=@sizeId", new { gId, sizeId }, conn: conn, tran: tran);
        }
        public async Task<int> AddRelation(IEnumerable<GoodsRelationViewModel> goodsRelationDatas, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await Create(goodsRelationDatas, conn, tran);
        }
    }
}
