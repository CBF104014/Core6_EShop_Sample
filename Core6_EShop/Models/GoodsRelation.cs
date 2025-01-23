using System.ComponentModel.DataAnnotations;

namespace Core6_EShop.Models
{
    public class GoodsRelation
    {
        public long rankey { get; set; }
        public int gId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "必須選取任一項")]
        public int sizeId { get; set; }
        //[Range(1, int.MaxValue, ErrorMessage = "數值須大於0")]
        public int stockNum { get; set; }
    }
}
