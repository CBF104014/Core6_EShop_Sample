using Newtonsoft.Json;
using System.Collections;
using System.Xml.Linq;

namespace Core6_EShop.Cls
{
    public static class Code
    {
        public enum stateCode
        {
            error = -1,
            info = 0,
            success = 1,
            warning = 2,
            question = 3,
        }
        public static T Mapping<T>(this object _data)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(_data));
        }
        public static T Init<T>(this T _data)
        {
            try
            {
                if (_data == null)
                    return _data;
                if (_data.GetType().IsGenericType && _data is IEnumerable)
                {
                    for (int i = 0; i < ((IList)_data).Count; i++)
                        ((IList)_data)[i].Init();
                }
                else
                {
                    var properties = _data.GetType().GetProperties();
                    foreach (var o in properties)
                    {
                        if (o.GetSetMethod(false) == null)
                            continue;
                        var name = o.Name;
                        var obj = _data.GetType().GetProperty(name);
                        var val = obj.GetValue(_data, null);
                        if (val == null)
                        {
                            if (o.PropertyType == typeof(int) || o.PropertyType == typeof(decimal)) { obj.SetValue(_data, 0); }
                            else if (o.PropertyType == typeof(string)) { obj.SetValue(_data, ""); }
                            else if (o.PropertyType == typeof(bool)) { obj.SetValue(_data, false); }
                            else { }
                        }
                    }
                }
                return _data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
