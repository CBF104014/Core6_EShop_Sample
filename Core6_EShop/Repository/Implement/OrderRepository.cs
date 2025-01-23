using Core6_EShop.Cls;
using Core6_EShop.Dto;
using Core6_EShop.Models;
using Core6_EShop.Repository.Base;
using Core6_EShop.ViewModel;
using System.Data;

namespace Core6_EShop.Repository.Implement
{
    public class OrderRepository : BaseRepository<Order>
    {
        private GoodsRelationRepository goodsRelationRepository;
        private OrderRelationRepository orderRelationRepository;
        private CartRepository cartRepository;
        public OrderRepository(OrderRelationRepository orderRelationRepository, CartRepository cartRepository, GoodsRelationRepository goodsRelationRepository)
        {
            this.orderRelationRepository = orderRelationRepository;
            this.cartRepository = cartRepository;
            this.goodsRelationRepository = goodsRelationRepository;
        }
        /// <summary>
        /// 訂單Grid
        /// </summary>
        public async Task<DataTablesResultDto> GetOrderGrid(int memberId,DataTablesDto dataTablesDto)
        {
            return await GetBaseGrid(dataTablesDto, SelByMemberId(memberId));
        }
        public Order GetSampleData(int memberId)
        {
            return new Order()
            {
                memberId = memberId,
            }.Init();
        }
        public async Task<long> GetNewOrderId(int orderYear, IDbConnection conn = null, IDbTransaction tran = null)
        {
            var sqlCmd = $"select ifnull(max(orderId),0) orderId from {Code.tableCode.Order.TableFullName} where YEAR(orderDate)=@orderYear;";
            var maxOrderId = await QueryFirstAsync<long>(sqlCmd, new
            {
                orderYear,
            }, conn: conn, tran: tran);
            var baseNum = 10000000L;
            if (maxOrderId == 0)
            {
                maxOrderId = DateTime.Now.Year * baseNum;
            }
            return maxOrderId + 1;
        }
        public async Task<IEnumerable<Order>> SelByMemberId(int memberId, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await SelAsync<Order>($"memberId=@memberId", new { memberId }, conn: conn, tran: tran);
        }
        public async Task<IEnumerable<OrderViewModel>> SelByMemberIdToViewModel(int memberId, IDbConnection conn = null, IDbTransaction tran = null)
        {
            var orderDatas = await SelAsync<OrderViewModel>($"memberId=@memberId", new { memberId });
            var orderRelationDatas = await orderRelationRepository
                .SelByOrderIds(orderDatas.Select(x => x.orderId));
            foreach (var item in orderDatas)
            {
                item.orderRelationDatas = orderRelationDatas
                    .Where(x => x.orderId == item.orderId);
            }
            return orderDatas;
        }
        public async Task<APIDto> SaveData(int memberId, OrderDto orderDtoData, IDbConnection conn = null, IDbTransaction tran = null)
        {
            var orderRelationDatas = orderRelationRepository
                .GetFromCart(orderDtoData.cartDatas)
                .ToList();
            var goodsRelationDatas = await goodsRelationRepository
                .SelMultipleBySizeIdToViewModel(orderRelationDatas.Select(x => x.gId).ToArray(), orderRelationDatas.Select(x => x.sizeId).ToArray());
            using (conn = GetConnection(conn))
            {
                conn.Open();
                using (tran = GetTransaction(conn, tran))
                {
                    var nowTime = DateTime.Now;
                    orderDtoData.orderData.orderId = await GetNewOrderId(nowTime.Year, conn: conn, tran: tran);
                    orderDtoData.orderData.memberId = memberId;
                    orderDtoData.orderData.orderState = Code.orderStateCode.progress.varValue;
                    orderDtoData.orderData.orderDate = nowTime;
                    orderDtoData.orderData.orderFinishDate = null; 
                    orderDtoData.orderData.orderAmount = 0;
                    foreach (var item in orderRelationDatas)
                    {
                        item.orderId = orderDtoData.orderData.orderId;
                        orderDtoData.orderData.orderAmount += item.orderItemPrice * item.orderQuantity;
                        var goodsRelationData = goodsRelationDatas
                            .FirstOrDefault(x => x.gId == item.gId && x.sizeId == item.sizeId);
                        if (goodsRelationData != null)
                        {
                            var currentStockNum = goodsRelationData.stockNum;
                            goodsRelationData.stockNum = currentStockNum - item.orderQuantity;
                            if (goodsRelationData.stockNum < 0)
                            {
                                tran.Rollback();
                                $"庫存不足。商品編號：{item.gId}；商品規格：{item.sizeId}；目前庫存：{currentStockNum}；使用者購買量：{item.orderQuantity}".ToLog();
                                return new APIDto((int)Code.stateCode.error, "庫存不足", "", new { });
                            }
                            await goodsRelationRepository.UpdByRankey(goodsRelationData, conn: conn, tran: tran);
                        }
                    }
                    await Create(orderDtoData.orderData, conn: conn, tran: tran);
                    await orderRelationRepository.Create(orderRelationDatas, conn: conn, tran: tran);
                    await cartRepository.DelByRankeys(orderDtoData.cartDatas.Select(x => x.rankey), conn: conn, tran: tran);
                    tran.Commit();
                }
            }
            return new APIDto((int)Code.stateCode.success, "訂單送出成功", "", new { });
        }
    }
}
