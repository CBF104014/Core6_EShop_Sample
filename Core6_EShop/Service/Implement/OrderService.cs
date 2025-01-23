using Core6_EShop.Dto;
using Core6_EShop.Models;
using Core6_EShop.Repository.Implement;
using Core6_EShop.Service.Base;
using Core6_EShop.ViewModel;
using System.Data;

namespace Core6_EShop.Service.Implement
{
    public class OrderService : BaseService<Order>
    {
        private OrderRepository orderRepository { get; set; }
        public OrderService(OrderRepository orderRepository) : base(orderRepository)
        {
            this.orderRepository = orderRepository;
        }
        public async Task<DataTablesResultDto> GetOrderGrid(int memberId, DataTablesDto dataTablesDto)
        {
            return await orderRepository.GetOrderGrid(memberId, dataTablesDto);
        }
        public Order GetSampleData(int memberId)
        {
            return orderRepository.GetSampleData(memberId);
        }
        public async Task<long> GetNewOrderId(int orderYear, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await orderRepository.GetNewOrderId(orderYear, conn: conn, tran: tran);
        }
        public async Task<IEnumerable<Order>> SelByMemberId(int memberId, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await orderRepository.SelByMemberId(memberId, conn: conn, tran: tran);
        }
        public async Task<IEnumerable<OrderViewModel>> SelByMemberIdToViewModel(int memberId, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await orderRepository.SelByMemberIdToViewModel(memberId, conn: conn, tran: tran);
        }
        public async Task<APIDto> SaveData(int memberId, OrderDto orderDtoData, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await orderRepository.SaveData(memberId, orderDtoData, conn: conn, tran: tran);
        }
    }
}
