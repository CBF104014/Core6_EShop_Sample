using Core6_EShop.Models;
using Core6_EShop.Repository.Implement;
using Core6_EShop.Service.Base;
using Core6_EShop.ViewModel;

namespace Core6_EShop.Service.Implement
{
    public class OrderRelationService : BaseService<OrderRelation>
    {
        private OrderRelationRepository orderRelationRepository { get; set; }
        public OrderRelationService(OrderRelationRepository orderRelationRepository) : base(orderRelationRepository)
        {
            this.orderRelationRepository = orderRelationRepository;
        }
        public async Task<IEnumerable<OrderRelationViewModel>> SelByOrderIdToViewModel(long orderId, IWebHostEnvironment env)
        {
            return await orderRelationRepository.SelByOrderIdToViewModel(orderId, env);
        }
    }
}
