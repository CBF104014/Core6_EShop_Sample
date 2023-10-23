namespace Core6_EShop.DB.Interface
{
    public interface ISQLTool<T>
    {
        int Create(T data);
        IEnumerable<T> Read(string whereCmd = "", Dictionary<string, object> sqlParam = null);
        int Update(T Data, string whereCmd = "", Dictionary<string, object> sqlParam = null);
        int Delete(string whereCmd = "", Dictionary<string, object> sqlParam = null);
        int Execute(string sqlCmd, Dictionary<string, object> sqlParam = null);
        IEnumerable<T1> Query<T1>(string sqlCmd, Dictionary<string, object> sqlParam = null);
    }
}
