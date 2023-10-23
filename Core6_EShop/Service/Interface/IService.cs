namespace Core6_EShop.Service.Interface
{
    public interface IService<T>
    {
        IEnumerable<T> Sel(string whereCmd = "", Dictionary<string, object> sqlParam = null);
    }
}
