using System.ComponentModel.DataAnnotations;

namespace Core6_EShop.Models
{
    public class Goods
    {
        public long rankey { get; set; }
        public int gId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "欄位必填")]
        public int gType1 { get; set; }
        [Required(ErrorMessage = "欄位必填")]
        public string goodName { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "數值須大於0")]
        public decimal goodPrice { get; set; }
        [Required(ErrorMessage = "欄位必填")]
        public string goodDesc { get; set; }
        public int goodState { get; set; }
    }
}
