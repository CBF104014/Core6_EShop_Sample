using Core6_EShop.Repository.Interface;
using Core6_EShop.Service.Interface;
using System.Data;

namespace Core6_EShop.Service.Base
{
    public class BaseService<T> : IService<T> where T : class
    {
        private IRepository<T> repository { get; set; }
        public BaseService(IRepository<T> repository)
        {
            this.repository = repository;
        }
        public async Task<T> SelByRankey(long rankey, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await repository.SelByRankey(rankey, conn: conn, tran: tran);
        }

        public async Task<IEnumerable<T1>> SelAllAsync<T1>(int start = 0, int count = 0, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await repository.SelAllAsync<T1>(start, count, conn: conn, tran: tran);
        }
    }
    //public class BaseService<T> where T : class
    //{
    //    private string connStr { get; set; }
    //    public MySqlTool<T> _mySqlTool { get; set; }
    //    public BaseService()
    //    {
    //        this.connStr = "server=p3nlmysql181plsk.secureserver.net;uid=goose;pwd=John104014;database=GooseDB";
    //        this._mySqlTool = new MySqlTool<T>(this.connStr);
    //    }
    //    public T SelByRankey(long rankey)
    //    {
    //        return _mySqlTool.Sel("Rankey=@Rankey", new Dictionary<string, object>()
    //        {
    //            ["Rankey"] = rankey,
    //        }).FirstOrDefault();
    //    }
    //    public IEnumerable<T> SelAll(int start = 0, int count = 0)
    //    {
    //        var sqlCmd = $"select * from {_mySqlTool.FullName} {(count == 0 ? "" : $"limit {start},{count}")}";
    //        return _mySqlTool.Query<T>(sqlCmd);
    //    }
    //    public IEnumerable<T1> SelAll<T1>(int start = 0, int count = 0)
    //    {
    //        var sqlCmd = $"select * from {_mySqlTool.FullName} {(count == 0 ? "" : $"limit {start},{count}")}";
    //        return _mySqlTool.Query<T1>(sqlCmd);
    //    }
    //    public int GetCount()
    //    {
    //        var sqlCmd = $"select count(Rankey) count from {_mySqlTool.FullName}";
    //        return _mySqlTool.Query<int>(sqlCmd).First();
    //    }
    //}
}
