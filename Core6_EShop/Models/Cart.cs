using System.ComponentModel.DataAnnotations;

namespace Core6_EShop.Models
{
    public class Cart
    {
        public long rankey { get; set; }
        public int memberId { get; set; }
        public int gId { get; set; }
        public int sizeId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "數量不可低於0")]
        public int quantity { get; set;}
        public bool isChecked { get; set;}
    }
}
