using Core6_EShop.Models;

namespace Core6_EShop.ViewModel
{
    public class GoodsViewModel: Goods
    {
        public FileViewModel fileData { get; set; }
        public string basePath { get; set; }
        public string imagePath
        {
            get
            {
                return Path.Combine("image", "goods", this.GType1 ?? "", this.GType2 ?? "", this.GId.ToString("00000"));
            }
        }
        public string fullPathWithoutImage
        {
            get
            {
                return Path.Combine(this.basePath ?? "", this.imagePath);
            }
        }
        public string fullPath
        {
            get
            {
                return Path.Combine(this.fullPathWithoutImage, $"main.jpg");
            }
        }
    }
}
