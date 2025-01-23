using Core6_EShop.Models;

namespace Core6_EShop.ViewModel
{
    public class OrderViewModel : Order
    {
        public IEnumerable<OrderRelation> orderRelationDatas { get; set; }
    }
}
