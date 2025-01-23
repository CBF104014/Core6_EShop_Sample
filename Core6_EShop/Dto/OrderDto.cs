using Core6_EShop.Models;
using Core6_EShop.ViewModel;

namespace Core6_EShop.Dto
{
    public class OrderDto
    {
        public Order orderData { get; set; }
        public IEnumerable<CartViewModel> cartDatas { get; set; }
    }
}
