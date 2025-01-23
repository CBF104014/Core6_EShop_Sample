using Core6_EShop.Models;
using Core6_EShop.ModelValidator;
using System.ComponentModel.DataAnnotations;

namespace Core6_EShop.ViewModel
{
    public class GoodsViewModel: Goods
    {
        [UniqueItemValidator("資料不可重複", "sizeId")]
        [Required(ErrorMessage = "至少新增一筆資料")]
        public List<GoodsRelationViewModel> goodsSizeDatas { get; set; }
        public int sizeId { get; set; }
        public string sizeName { get; set; } = string.Empty;
        public int goodsQuantity { get; set; }
        public string basePath { get; set; } = "";
        public string imagePath
        {
            get
            {
                return Path.Combine("image", "goods", this.gId.ToString());
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
                return Path.Combine(this.fullPathWithoutImage, $"main.png");
            }
        }
        public string thumbnail_sPath
        {
            get
            {
                return Path.Combine(this.fullPathWithoutImage, $"thumbnail_s.png");
            }
        }
        public string thumbnail_mPath
        {
            get
            {
                return Path.Combine(this.fullPathWithoutImage, $"thumbnail_m.png");
            }
        }
    }
}
