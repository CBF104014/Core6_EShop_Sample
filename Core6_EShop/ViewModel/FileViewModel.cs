using System.ComponentModel.DataAnnotations;

namespace Core6_EShop.ViewModel
{
    public class FileViewModel
    {
        public long rankey { get; set; }
        public int fileId { get; set; }
        public string fileName { get; set; }
        public string fileType { get; set; }
        [Required(ErrorMessage = "欄位必填")]
        public byte[] byteData { get; set; }
    }
}
