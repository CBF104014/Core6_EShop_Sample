using Core6_EShop.DB.Interface;
using System.Reflection;
using Dapper;
using MySql.Data.MySqlClient;
using Core6_EShop.Cls;

namespace Core6_EShop.DB.Implement
{
    public class MySqlTool<T> : ISQLTool<T>
    {
        private string connStr { get; set; }
        public string TableName { get { return typeof(T).Name; } }
        public MySqlTool()
        {
            this.connStr = "server=p3nlmysql181plsk.secureserver.net;uid=goose;pwd=John104014;database=GooseDB";
        }
        public int Create(T data)
        {
            var propertyDatas = typeof(T)
                .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name != "Rankey")
                .Select(x => x.Name);
            var columnCmdStr = String.Join(",", propertyDatas.Select(x => $"{x}"));
            var valueCmdStr = String.Join(",", propertyDatas.Select(x => $"@{x}"));
            var sqlCmd = $"insert into {TableName} ({columnCmdStr}) values ({valueCmdStr})";
            var dictData = data.Mapping<Dictionary<string, object>>();
            return this.Execute(sqlCmd, dictData);
        }

        public int Delete(string whereCmd = "", Dictionary<string, object> sqlParam = null)
        {
            var sqlCmd = $"delete {this.TableName} {(String.IsNullOrEmpty(whereCmd) ? "" : $"where {whereCmd}")}";
            return this.Execute(sqlCmd);
        }

        public int Execute(string sqlCmd, Dictionary<string, object> sqlParam = null)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                return conn.Execute(sqlCmd, sqlParam);
            }
        }

        public IEnumerable<T1> Query<T1>(string sqlCmd, Dictionary<string, object> sqlParam = null)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                return conn.Query<T1>(sqlCmd, sqlParam);
            }
        }

        public IEnumerable<T> Read(string whereCmd = "", Dictionary<string, object> sqlParam = null)
        {
            var sqlCmd = $"select * from {TableName} {(String.IsNullOrEmpty(whereCmd) ? "" : $"where {whereCmd}")}";
            using (var conn = new MySqlConnection(connStr))
            {
                return conn.Query<T>(sqlCmd, sqlParam);
            }
        }

        public int Update(T Data, string whereCmd = "", Dictionary<string, object> sqlParam = null)
        {
            var sqlCmd = $"update {this.TableName} set(...) {(String.IsNullOrEmpty(whereCmd) ? "" : $"where {whereCmd}")}";
            return this.Execute(sqlCmd);
        }
    }
}
