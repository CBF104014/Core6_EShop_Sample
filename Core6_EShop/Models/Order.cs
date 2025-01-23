using System.ComponentModel.DataAnnotations;

namespace Core6_EShop.Models
{
    public class Order
    {
        public long rankey { get; set; }
        public long orderId { get; set; }
        public int memberId { get; set; }
        public int orderState { get; set; }
        public string orderDesc { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "欄位必填")]
        public int paymentType { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "欄位必填")]
        public int deliveryType { get; set; }
        public DateTime? orderDate { get; set; }
        public DateTime? orderFinishDate { get; set; }
        public decimal orderAmount { get; set; }
    }
}
