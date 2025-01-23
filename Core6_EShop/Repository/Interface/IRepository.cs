using System.Data;

namespace Core6_EShop.Repository.Interface
{
    public interface IRepository<T>
    {
        Task<T> SelByRankey(long rankey, IDbConnection conn = null, IDbTransaction tran = null);
        Task<IEnumerable<T1>> SelAllAsync<T1>(int start = 0, int count = 0, IDbConnection conn = null, IDbTransaction tran = null);
    }
}
