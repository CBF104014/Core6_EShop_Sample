using Core6_EShop.DB.Implement;
using Core6_EShop.Models;
using Core6_EShop.Service.Interface;

namespace Core6_EShop.Service.Implement
{
    public class MemberService
    {
        private MySqlTool<Member> _mySqlTool { get; set; }
        public MemberService()
        {
            this._mySqlTool = new MySqlTool<Member>();
        }
        public Member SelById(string Id)
        {
            return _mySqlTool.Read("Id=@Id", new Dictionary<string, object>()
            {
                ["Id"] = Id,
            }).FirstOrDefault();
        }
    }
}
