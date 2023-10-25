using Core6_EShop.DB.Implement;
using Core6_EShop.Dto;
using Core6_EShop.Models;
using Core6_EShop.ViewModel;

namespace Core6_EShop.Service.Base
{
    public class BaseService<T> where T : class
    {
        public MySqlTool<T> _mySqlTool { get; set; }
        public BaseService()
        {
            this._mySqlTool = new MySqlTool<T>();
        }
        public T SelByRankey(int rankey)
        {
            return _mySqlTool.Read("Rankey=@Rankey", new Dictionary<string, object>()
            {
                ["Rankey"] = rankey,
            }).FirstOrDefault();
        }
        public IEnumerable<T> SelAll(int start = 0, int count = 0)
        {
            var sqlCmd = $"select * from {_mySqlTool.TableName} {(count == 0 ? "" : $"limit {start},{count}")}";
            return _mySqlTool.Query<T>(sqlCmd);
        }
        public int GetCount()
        {
            var sqlCmd = $"select count(Rankey) count from {_mySqlTool.TableName}";
            return _mySqlTool.Query<int>(sqlCmd).First();
        }
    }
}
