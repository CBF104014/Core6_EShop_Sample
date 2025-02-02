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
        public async Task<int> UpdByRankey(T data, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await repository.UpdByRankey(data, conn: conn, tran: tran);
        }
        public async Task<int> DelByRankey(long rankey, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await repository.DelByRankey(rankey, conn: conn, tran: tran);
        }
    }
}
