using Core6_EShop.Cls;
using Core6_EShop.Dto;
using Core6_EShop.Repository.Interface;
using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection;

namespace Core6_EShop.Repository.Base
{
    /* mysql不支援MARS */
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private string dbName { get { return "GooseDB"; } }
        private string projectName { get { return "EShop"; } }
        public string tableName { get { return typeof(T).Name; } }
        public string fullName { get { return $"{this.dbName}.{this.projectName}_{this.tableName}"; } }
        private string connStr { get; set; }
        private IEnumerable<string> propertyDatas { get; set; }
        public BaseRepository()
        {
            this.connStr = Code.mySQLGooseDBConnStr;
            this.propertyDatas = typeof(T)
                .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.ToLower() != "rankey")
                .Select(x => x.Name);
        }
        public Task<T> SelByRankey(long rankey, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return SelFirst<T>("rankey=@rankey", new { rankey }, conn, tran);
        }
        public Task<IEnumerable<T1>> SelAllAsync<T1>(int start = 0, int count = 0, IDbConnection conn = null, IDbTransaction tran = null)
        {
            var sqlCmd = $"select * from {this.fullName} {(count == 0 ? "" : $"limit @start,@count")}";
            return QueryAsync<T1>(sqlCmd, new { start, count }, conn, tran);
        }
        public IEnumerable<T1> SelAll<T1>(int start = 0, int count = 0, IDbConnection conn = null, IDbTransaction tran = null)
        {
            var sqlCmd = $"select * from {this.fullName} {(count == 0 ? "" : $"limit @start,@count")}";
            return Query<T1>(sqlCmd, new { start, count }, conn, tran);
        }
        /// <summary>
        /// 取得連線
        /// </summary>
        protected IDbConnection GetConnection(IDbConnection conn = null)
        {
            return conn == null ? new MySqlConnection(this.connStr) : conn;
        }
        /// <summary>
        /// 取得交易
        /// </summary>
        protected IDbTransaction GetTransaction(IDbConnection conn, IDbTransaction tran = null)
        {
            return tran == null ? GetConnection(conn).BeginTransaction() : tran;
        }
        /// <summary>
        /// 新增
        /// </summary>
        public Task<int> Create(T data, IDbConnection conn = null, IDbTransaction tran = null)
        {
            var columnCmdStr = String.Join(",", propertyDatas.Select(x => $"`{x}`"));
            var valueCmdStr = String.Join(",", propertyDatas.Select(x => $"@{x}"));
            var sqlCmd = $"insert into {this.fullName} ({columnCmdStr}) values ({valueCmdStr});";
            return this.Execute(sqlCmd, data, conn, tran);
        }
        public Task<int> Create(IEnumerable<T> datas, IDbConnection conn = null, IDbTransaction tran = null)
        {
            var columnCmdStr = String.Join(",", propertyDatas.Select(x => $"`{x}`"));
            var count = datas.Count();
            var sqlCmd = "";
            var sqlParameter = new Dictionary<string, object>();
            for (int i = 0; i < count; i++)
            {
                var valueCmdStr = String.Join(",", propertyDatas.Select(x => $"@{x}_{i}"));
                var dictData = datas.ElementAt(i)
                    .Mapping<T>()
                    .Mapping<Dictionary<string, object>>();
                foreach (var item in dictData)
                {
                    sqlParameter.Add($"{item.Key}_{i}", item.Value);
                }
                sqlCmd += $"insert into {this.fullName} ({columnCmdStr}) values ({valueCmdStr}); \r\n";
            }
            return this.Execute(sqlCmd, sqlParameter, conn, tran);
        }
        /// <summary>
        /// 刪除
        /// </summary>
        public Task<int> Delete(string whereCmd = "", object sqlParam = null, IDbConnection conn = null, IDbTransaction tran = null)
        {
            var sqlCmd = $"delete from {this.fullName} {(String.IsNullOrEmpty(whereCmd) ? "" : $"where {whereCmd}")};";
            return this.Execute(sqlCmd, sqlParam, conn, tran);
        }
        public Task<int> DelByRankey(long rankey, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return this.Delete("rankey=@rankey", new { rankey }, conn, tran);
        }
        public Task<int> DelByRankeys(IEnumerable<long> rankeyDatas, IDbConnection conn = null, IDbTransaction tran = null)
        {
            var sqlParameter = new Dictionary<string, object>();
            foreach (var item in rankeyDatas)
            {
                sqlParameter.Add($"rankey_{item}", item);
            }
            var whereCmd = $"rankey in ({String.Join(",", sqlParameter.Select(x => $"@{x.Key}"))})";
            return Delete(whereCmd, sqlParameter, conn: conn, tran: tran);
        }
        /// <summary>
        /// 執行
        /// </summary>
        public Task<int> Execute(string sqlCmd, object sqlParam = null, IDbConnection conn = null, IDbTransaction tran = null)
        {
            if (conn == null)
            {
                using (conn = GetConnection())
                {
                    return conn.ExecuteAsync(sqlCmd, sqlParam, transaction: tran);
                }
            }
            else
            {
                return conn.ExecuteAsync(sqlCmd, sqlParam, transaction: tran);
            }
        }
        /// <summary>
        /// 查詢多筆-整個SQL語法
        /// </summary>
        public IEnumerable<T1> Query<T1>(string sqlCmd, object sqlParam = null, IDbConnection conn = null, IDbTransaction tran = null)
        {
            if (conn == null)
            {
                using (conn = GetConnection())
                {
                    return conn.Query<T1>(sqlCmd, sqlParam, transaction: tran);
                }
            }
            else
            {
                return conn.Query<T1>(sqlCmd, sqlParam, transaction: tran);
            }
        }
        /// <summary>
        /// 查詢多筆-整個SQL語法
        /// </summary>
        public Task<IEnumerable<T1>> QueryAsync<T1>(string sqlCmd, object sqlParam = null, IDbConnection conn = null, IDbTransaction tran = null)
        {
            if (conn == null)
            {
                using (conn = GetConnection())
                {
                    return conn.QueryAsync<T1>(sqlCmd, sqlParam, transaction: tran);
                }
            }
            else
            {
                return conn.QueryAsync<T1>(sqlCmd, sqlParam, transaction: tran);
            }
        }
        /// <summary>
        /// 查詢單筆-整個SQL語法
        /// </summary>
        public Task<T1> QueryFirstAsync<T1>(string sqlCmd, object sqlParam = null, IDbConnection conn = null, IDbTransaction tran = null)
        {
            if (conn == null)
            {
                using (conn = GetConnection())
                {
                    return conn.QueryFirstOrDefaultAsync<T1>(sqlCmd, sqlParam, transaction: tran);
                }
            }
            else
            {
                return conn.QueryFirstOrDefaultAsync<T1>(sqlCmd, sqlParam, transaction: tran);
            }
        }
        /// <summary>
        /// 查詢多筆-WHERE語法
        /// </summary>
        public Task<IEnumerable<T1>> SelAsync<T1>(string whereCmd = "", object sqlParam = null, IDbConnection conn = null, IDbTransaction tran = null)
        {
            var sqlCmd = $"select * from {this.fullName} {(String.IsNullOrEmpty(whereCmd) ? "" : $"where {whereCmd}")}";
            return QueryAsync<T1>(sqlCmd, sqlParam, conn, tran);
        }
        /// <summary>
        /// 查詢單筆-WHERE語法
        /// </summary>
        public Task<T1> SelFirst<T1>(string whereCmd = "", object sqlParam = null, IDbConnection conn = null, IDbTransaction tran = null)
        {
            var sqlCmd = $"select * from {this.fullName} {(String.IsNullOrEmpty(whereCmd) ? "" : $"where {whereCmd}")} limit 1";
            return QueryFirstAsync<T1>(sqlCmd, sqlParam, conn, tran);
        }
        /// <summary>
        /// 更新資料
        /// </summary>
        public Task<int> Update(T data, string whereCmd = "", object sqlParam = null, IDbConnection conn = null, IDbTransaction tran = null)
        {
            var columnCmdStr = String.Join(",", propertyDatas.Select(x => $"`{x}`=@{x}"));
            var sqlCmd = $"update {this.fullName} set {columnCmdStr} {(String.IsNullOrEmpty(whereCmd) ? "" : $"where {whereCmd}")};";
            if (sqlParam == null)
            {
                return this.Execute(sqlCmd, data, conn, tran);
            }
            else
            {
                var updData = data.Mapping<Dictionary<string, object>>()
                    .Concat(sqlParam.Mapping<Dictionary<string, object>>())
                    .GroupBy(kv => kv.Key)
                    .ToDictionary(g => g.Key, g => g.Last().Value);
                return this.Execute(sqlCmd, updData, conn, tran);
            }
        }
        public Task<int> UpdByRankey(T data, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return this.Update(data, "rankey=@rankey", null, conn, tran);
        }
        /// <summary>
        /// 查詢筆數
        /// </summary>
        public Task<int> GetCount(string whereCmd = "", object sqlParam = null, IDbConnection conn = null, IDbTransaction tran = null)
        {
            var sqlCmd = $"select ifnull(count(*),0) count from {this.fullName}";
            if (conn == null)
            {
                using (conn = GetConnection())
                {
                    return conn.ExecuteScalarAsync<int>(sqlCmd, sqlParam, transaction: tran);
                }
            }
            else
            {
                return conn.ExecuteScalarAsync<int>(sqlCmd, sqlParam, transaction: tran);
            }
        }
        /// <summary>
        /// DataTables
        /// </summary>
        public async Task<DataTablesResultDto> GetBaseGrid<TResult>(DataTablesDto dataTablesDto, Task<IEnumerable<TResult>> allDatasTask)
        {
            var datas = await allDatasTask;
            return this.GetBaseGrid(dataTablesDto, datas);
        }
        /// <summary>
        /// DataTables
        /// </summary>
        public DataTablesResultDto GetBaseGrid<TResult>(DataTablesDto dataTablesDto, IEnumerable<TResult> allDatas)
        {
            var allPropertyName = typeof(TResult)
                    .GetProperties()
                    .Select(x => x.Name)
                    .ToList();
            //關鍵字
            if (!String.IsNullOrEmpty(dataTablesDto.searchValue))
            {
                allDatas = allDatas.DynamicSearch(dataTablesDto.searchValue);
            }
            // 排序
            if (!String.IsNullOrEmpty(dataTablesDto.sortColumn))
            {
                var isDesc = dataTablesDto.sortDirection.ToLower() == "desc";
                allDatas = allDatas.DynamicSort(dataTablesDto.sortColumn, isDesc);
            }
            //總筆數
            var totalCount = allDatas.Count();
            var resultData = new DataTablesResultDto()
            {
                draw = dataTablesDto.draw,
                recordsTotal = totalCount,
                recordsFiltered = totalCount,
                data = allDatas
                    .Skip(dataTablesDto.start)
                    .Take(dataTablesDto.length)
                    .ToList()
            };
            return resultData;
        }
    }
}
