using Core6_EShop.Cls;
using Core6_EShop.Models;
using Core6_EShop.Repository.Base;
using Core6_EShop.ViewModel;
using System.Data;

namespace Core6_EShop.Repository.Implement
{
    public class CartRepository : BaseRepository<Cart>
    {
        private GoodsRepository goodsRepository { get; set; }
        public CartRepository(GoodsRepository goodsRepository)
        {
            this.goodsRepository = goodsRepository;
        }
        #region 新增
        public async Task<int> AddToCart(Cart cartData, IDbConnection conn = null, IDbTransaction tran = null)
        {
            var cartDatas = await SelByMemberId(cartData.memberId, conn: conn, tran: tran);
            var currentCartData = cartDatas.FirstOrDefault(x => x.gId == cartData.gId && x.sizeId == cartData.sizeId);
            if (currentCartData == null)
            {
                //未存在
                if (cartData.quantity == 0)
                    cartData.quantity = 1;
                return await Create(cartData, conn: conn, tran: tran);
            }
            else
            {
                //已存在
                currentCartData.quantity++;
                return await UpdByRankey(currentCartData, conn: conn, tran: tran);
            }
        }
        #endregion
        #region 查詢
        public async Task<IEnumerable<Cart>> SelById(int memberId)
        {
            return await SelAsync<Cart>("memberId=@memberId", new { memberId });
        }
        public async Task<IEnumerable<Cart>> SelByMemberId(int memberId, IDbConnection conn = null, IDbTransaction tran = null)
        {
            var cartDatas = await SelAsync<Cart>($"memberId=@memberId", new
            {
                memberId,
            }, conn: conn, tran: tran);
            return cartDatas;
        }
        public async Task<IEnumerable<Cart>> SelByGid(int memberId, int gId, IDbConnection conn = null, IDbTransaction tran = null)
        {
            var cartDatas = await SelAsync<Cart>($"memberId=@memberId and gId=@gId", new
            {
                memberId,
                gId,
            }, conn: conn, tran: tran);
            return cartDatas;
        }
        public async Task<IEnumerable<CartViewModel>> SelToViewModelById(int memberId)
        {
            var cartDats = await SelAsync<CartViewModel>("memberId=@memberId", new { memberId });
            if (!cartDats.Any())
                return null;
            var goodsDatas = await goodsRepository.SelMultipleByGIdToViewModel(cartDats.Select(x => x.gId).ToArray());
            foreach (var item in cartDats)
            {
                item.goodsData = goodsDatas.FirstOrDefault(x => x.gId == item.gId);
            }
            return cartDats;
        }
        public async Task<IEnumerable<CartViewModel>> SelPreOrderById(int memberId)
        {
            var cartDatas = await SelToViewModelById(memberId);
            return cartDatas.Where(x => x.isChecked);
        }
        #endregion
        #region 編輯
        public async Task<int> UpdCart(Cart cartData, IDbConnection conn = null, IDbTransaction tran = null)
        {
            using (conn = GetConnection(conn))
            {
                conn.Open();
                using (tran = GetTransaction(conn, tran))
                {
                    var cartDatas = await SelByGid(cartData.memberId, cartData.gId, conn: conn, tran: tran);
                    var currentData = cartDatas.First(x => x.rankey == cartData.rankey);
                    if (cartData.sizeId != currentData.sizeId)
                    {
                        var newData = cartDatas.FirstOrDefault(x => x.sizeId == cartData.sizeId);
                        if (newData == null)
                        {
                            newData = currentData.CloneObj();
                            newData.sizeId = cartData.sizeId;
                            await UpdByRankey(newData, conn: conn, tran: tran);
                        }
                        else
                        {
                            newData.quantity += cartData.quantity;
                            await DelByRankey(currentData.rankey, conn: conn, tran: tran);
                            await UpdByRankey(newData, conn: conn, tran: tran);
                        }
                    }
                    else if (cartData.quantity == 0)
                    {
                        await DelByRankey(cartData.rankey, conn: conn, tran: tran);
                    }
                    else
                    {
                        await UpdByRankey(cartData, conn: conn, tran: tran);
                    }
                    tran.Commit();
                }
            }
            return (int)Code.stateCode.success;
        }
        public async Task<int> UpdCartChecked(int memberId, bool isChecked, IDbConnection conn = null, IDbTransaction tran = null)
        {
            var sqlCmd = $"update {this.fullName} set `isChecked`=@isChecked where memberId=@memberId;";
            return await Execute(sqlCmd, new { memberId, isChecked }, conn: conn, tran: tran);
        }
        #endregion
        #region 刪除
        #endregion
        #region 其他
        public Cart GetSampleData(int memberId = 0, int gId = 0, int sizeId = 0, int quantity = 0)
        {
            return new Cart()
            {
                memberId = memberId,
                gId = gId,
                sizeId = sizeId,
                quantity = quantity,
            }.Init();
        }
        #endregion
    }
}
