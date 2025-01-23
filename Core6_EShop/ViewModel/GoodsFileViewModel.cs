
using System.ComponentModel.DataAnnotations;

namespace Core6_EShop.ViewModel
{
    public class GoodsFileViewModel : GoodsViewModel
    {
        [Required(ErrorMessage = "欄位必填")]
        public FileViewModel fileData { get; set; }
    }
}
