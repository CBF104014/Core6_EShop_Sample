using Core6_EShop.Models;
using Core6_EShop.Service.Base;
using Core6_EShop.ViewModel;

namespace Core6_EShop.Service.Implement
{
    public class CartService : BaseService<Cart>
    {
        public IEnumerable<CartViewModel> SelById(string id)
        {
            var sqlCmd = $@"
                select 
                    a.*,b.Name,c.GoodName,c.GoodPrice,c.GoodDesc
                from Cart a 
                left join Member b on a.Id=b.Id
                left join Goods c on a.GId=c.GId and a.GType1=c.GType1 and a.GType2=c.GType2
                where a.Id=@Id";
            return _mySqlTool.Query<CartViewModel>(sqlCmd, new Dictionary<string, object>()
            {
                ["Id"] = id,
            });
        }
    }
}
