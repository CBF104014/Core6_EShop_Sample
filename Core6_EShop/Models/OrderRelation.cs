namespace Core6_EShop.Models
{
    public class OrderRelation
    {
        public long rankey { get; set; }
        public long orderId { get; set; }
        public int gId { get; set; }
        public int sizeId { get; set; }
        public int orderQuantity { get; set; }
        public decimal orderItemPrice { get; set; }
    }
}
