using Core6_EShop.Models;

namespace Core6_EShop.ViewModel
{
    public class CartViewModel : Cart
    {
        public string Name { get; set; }
        public string GoodName { get; set; }
        public decimal GoodPrice { get; set; }
        public string GoodDesc { get; set; }
    }
}
