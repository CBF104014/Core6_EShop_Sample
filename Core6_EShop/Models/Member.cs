using System.ComponentModel.DataAnnotations;

namespace Core6_EShop.Models
{
    public class Member
    {
        public long rankey { get; set; }
        public int memberId { get; set; }
        [Required(ErrorMessage = "欄位必填")]
        public string email { get; set; }
        [Required(ErrorMessage = "欄位必填")]
        public string name { get; set; }
        public int zipCode { get; set; }
        public string countryCode { get; set; }
        public string city { get; set; }
        [Required(ErrorMessage = "欄位必填")]
        public string address { get; set; }
        [Required(ErrorMessage = "欄位必填")]
        public string phone { get; set; }
        public int memberState { get; set; }
        public int memberRight { get; set; }
    }
}
