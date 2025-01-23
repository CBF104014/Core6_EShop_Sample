using Core6_EShop.Cls;
using Core6_EShop.Models;
using Core6_EShop.Repository.Base;
using Core6_EShop.ViewModel;
using System.Data;

namespace Core6_EShop.Repository.Implement
{
    public class OrderRelationRepository : BaseRepository<OrderRelation>
    {
        private CartRepository cartRepository { get; set; }
        private GoodsRepository goodsRepository { get; set; }
        private GoodsSizeRepository goodsSizeRepository { get; set; }
        public OrderRelationRepository(CartRepository cartRepository, GoodsRepository goodsRepository, GoodsSizeRepository goodsSizeRepository)
        {
            this.cartRepository = cartRepository;
            this.goodsRepository = goodsRepository;
            this.goodsSizeRepository = goodsSizeRepository;
        }
        public OrderRelation GetSampleData()
        {
            return new OrderRelation()
            {
            }.Init();
        }
        public IEnumerable<OrderRelation> GetFromCart(IEnumerable<CartViewModel> cartDatas)
        {
            return cartDatas
                .Where(x => x.isChecked)
                .Select(x => new OrderRelation()
                {
                    gId = x.gId,
                    sizeId = x.sizeId,
                    orderQuantity = x.quantity,
                    orderItemPrice = x.goodsData.goodPrice,
                });
        }
        public async Task<IEnumerable<OrderRelation>> SelByOrderIds(IEnumerable<long> orderIdDatas)
        {
            var sqlParameter = new Dictionary<string, object>();
            var index = 0;
            foreach (var item in orderIdDatas)
            {
                sqlParameter.Add($"orderId_{index++}", item);
            }
            var whereCmd = $" orderId in ({String.Join(",", sqlParameter.Select(x => $"@{x.Key}"))});";
            return await SelAsync<OrderRelation>(whereCmd, sqlParameter);
        }
        public async Task<IEnumerable<OrderRelationViewModel>> SelByOrderIdToViewModel(long orderId, IWebHostEnvironment env)
        {
            var orderRelationDatas = await SelAsync<OrderRelationViewModel>("orderId=@orderId", new { orderId });
            var allGoodsDatas = await goodsRepository.SelMultipleByGIdToViewModel(orderRelationDatas.Select(x => x.gId).ToArray());
            var goodsSizeDatas = await goodsSizeRepository.SelAllAsync<GoodsSize>();
            foreach (var item in orderRelationDatas)
            {
                item.goodsData = allGoodsDatas.FirstOrDefault(x => x.gId == item.gId);
                if (item.goodsData != null)
                {
                    item.goodsData.sizeId = item.sizeId;
                    item.goodsData.sizeName = goodsSizeDatas
                        .First(x => x.sizeId == item.sizeId).sizeName;
                }
            }
            return orderRelationDatas;
        }
    }
}
