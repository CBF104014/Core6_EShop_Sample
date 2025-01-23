using Core6_EShop.Cls;

namespace Core6_EShop.Dto
{
    public class PageDto
    {
        public int pageNum { get; set; }
        public bool active { get; set; }
    }
    public class ShopDto
    {
        public int pageCount { get; set; }
        public int currentPageNum { get; set; }
        public int currentPageIndex { get => (currentPageNum - 1) * perPageNum; }
        public int perPageNum { get; set; }
        public string sortType { get; set; }
        public string searchKey { get; set; }
        public decimal minPrice { get; set; }
        public decimal maxPrice { get; set; }
        public int goodState { get; set; } = Code.goodStateCode.enable.varValue;
        public List<PageDto> pageCountList
        {
            get
            {
                var Datas = new List<PageDto>();
                for (int i = 0; i < pageCount; i++)
                {
                    Datas.Add(new PageDto
                    {
                        pageNum = i + 1,
                        active = (i + 1) == currentPageNum,
                    });
                }
                return Datas;
            }
        }
        public List<int> perPageNumList
        {
            get
            {
                return new List<int>()
                {
                    10, 20, 40, 60, 100
                };
            }
        }
        public List<KeyValuePair<string, string>> sortList
        {
            get
            {
                return new List<KeyValuePair<string, string>>()
                {
                    Code.shopSortCode.@default,
                    Code.shopSortCode.nameAsc,
                    Code.shopSortCode.nameDesc,
                    Code.shopSortCode.priceAsc,
                    Code.shopSortCode.priceDesc,
                };
            }
        }
    }
}
