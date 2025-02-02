using Core6_EShop.Dto;
using Core6_EShop.Models;
using Core6_EShop.Repository.Implement;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using System.Collections;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace Core6_EShop.Cls
{
    public static class Code
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public static void InitCode(IConfiguration configuration)
        {
            tokenKey = configuration.GetSection("AppSettings:Token").Value;
            tokenAddMinutes = Convert.ToInt32(configuration.GetSection("AppSettings:TokenAddMinutes").Value);
            appMaxRequestBodySize = Convert.ToInt64(configuration.GetSection("AppSettings:AppMaxRequestBodySize").Value);
            mySQLGooseDBConnStr = configuration.GetSection("MySQLConnectionString:GooseDB").Value;
            settingCodeDatas = new SettingRepository().SelAll<Setting>().ToList();
        }
        #region enum
        /// <summary>
        /// response狀態(配合前端sweetalert)
        /// </summary>
        public enum stateCode
        {
            error = -1,
            info = 0,
            success = 1,
            warning = 2,
            question = 3,
        }
        /// <summary>
        /// 代碼檔狀態
        /// </summary>
        public enum varStateCode
        {
            none = 0,
            enable = 1,
        }
        #endregion

        #region object
        /// <summary>
        /// 資料庫名稱代碼
        /// </summary>
        public static class dbCode
        {
            public static string GooseDB { get => "GooseDB"; }
        }
        /// <summary>
        /// 系統名稱代碼
        /// </summary>
        public static class systemCode
        {
            public static string EShop { get => "EShop"; }
        }
        /// <summary>
        /// 資料表代碼
        /// </summary>
        public static class tableCode
        {
            public static TableCodeDto Setting { get; } = new TableCodeDto("Setting", $"{dbCode.GooseDB}.{systemCode.EShop}_Setting");
            public static TableCodeDto Member { get; } = new TableCodeDto("Member", $"{dbCode.GooseDB}.{systemCode.EShop}_Member");
            public static TableCodeDto Goods { get; } = new TableCodeDto("Goods", $"{dbCode.GooseDB}.{systemCode.EShop}_Goods");
            public static TableCodeDto Cart { get; } = new TableCodeDto("Cart", $"{dbCode.GooseDB}.{systemCode.EShop}_Cart");
            public static TableCodeDto GoodsRelation { get; } = new TableCodeDto("GoodsRelation", $"{dbCode.GooseDB}.{systemCode.EShop}_GoodsRelation");
            public static TableCodeDto GoodsSize { get; } = new TableCodeDto("GoodsSize", $"{dbCode.GooseDB}.{systemCode.EShop}_GoodsSize");
            public static TableCodeDto Country { get; } = new TableCodeDto("Country", $"{dbCode.GooseDB}.{systemCode.EShop}_Country");
            public static TableCodeDto Order { get; } = new TableCodeDto("Order", $"{dbCode.GooseDB}.{systemCode.EShop}_Order");
            public static TableCodeDto OrderRelation { get; } = new TableCodeDto("OrderRelation", $"{dbCode.GooseDB}.{systemCode.EShop}_OrderRelation");
            public static TableCodeDto Arduinoa1 { get; } = new TableCodeDto("Arduinoa1", $"{dbCode.GooseDB}.Arduinoa1");
        }
        /// <summary>
        /// 商品排序代碼
        /// </summary>
        public static class shopSortCode
        {
           public static KeyValuePair<string, string> @default { get => new KeyValuePair<string, string>("default", "預設排序"); }
           public static KeyValuePair<string, string> nameAsc { get => new KeyValuePair<string, string>("nameAsc", "依名稱遞增"); }
           public static KeyValuePair<string, string> nameDesc { get => new KeyValuePair<string, string>("nameDesc", "依名稱遞減"); }
           public static KeyValuePair<string, string> priceAsc { get => new KeyValuePair<string, string>("priceAsc", "依金額遞減"); }
           public static KeyValuePair<string, string> priceDesc { get => new KeyValuePair<string, string>("priceDesc", "依金額遞減"); }
        }
        #endregion

        #region setting
        /// <summary>
        /// Token存活時間(分鐘)
        /// </summary>
        private static double tokenAddMinutes { get; set; }
        /// <summary>
        /// Token私鑰
        /// </summary>
        public static string tokenKey { get; set; }
        /// <summary>
        /// MySql連線字串
        /// </summary>
        public static string mySQLGooseDBConnStr { get; set; }
        /// <summary>
        /// 系統最大請求長度
        /// </summary>
        public static long appMaxRequestBodySize { get; set; }
        /// <summary>
        /// 系統參數代碼
        /// </summary>
        private static List<Setting> settingCodeDatas { get; set; }
        /// <summary>
        /// 商品分類代碼
        /// </summary>
        public static class gType1Code
        {
            public static List<Setting> selfDatas { get => settingCodeDatas.Where(x => x.varKey == "gType1").ToList(); }
            public static List<Setting> enableDatas { get => selfDatas.Where(x => x.varState == (int)varStateCode.enable).ToList(); }
            public static Setting noodle { get => selfDatas.First(x => x.varName == "noodle"); }
            public static Setting toast { get => selfDatas.First(x => x.varName == "toast"); }
            public static Setting hamburger { get => selfDatas.First(x => x.varName == "hamburger"); }
            public static Setting drinks { get => selfDatas.First(x => x.varName == "drinks"); }
            public static Setting other { get => selfDatas.First(x => x.varName == "other"); }
        }
        /// <summary>
        /// 人員狀態代碼
        /// </summary>
        public static class memberStateCode
        {
            public static List<Setting> selfDatas { get => settingCodeDatas.Where(x => x.varKey == "memberState").ToList(); }
            public static List<Setting> enableDatas { get => selfDatas.Where(x => x.varState == (int)varStateCode.enable).ToList(); }
            public static Setting none { get => selfDatas.First(x => x.varName == "none"); }
            public static Setting enable { get => selfDatas.First(x => x.varName == "enable"); }
        }
        /// <summary>
        /// 商品狀態代碼
        /// </summary>
        public static class goodStateCode
        {
            public static List<Setting> selfDatas { get => settingCodeDatas.Where(x => x.varKey == "goodState").ToList(); }
            public static List<Setting> enableDatas { get => selfDatas.Where(x => x.varState == (int)varStateCode.enable).ToList(); }
            public static Setting none { get => selfDatas.First(x => x.varName == "none"); }
            public static Setting enable { get => selfDatas.First(x => x.varName == "enable"); }
        }
        /// <summary>
        /// 訂單狀態代碼
        /// </summary>
        public static class orderStateCode
        {
            public static List<Setting> selfDatas { get => settingCodeDatas.Where(x => x.varKey == "orderState").ToList(); }
            public static List<Setting> enableDatas { get => selfDatas.Where(x => x.varState == (int)varStateCode.enable).ToList(); }
            public static Setting none { get => selfDatas.First(x => x.varName == "none"); }
            public static Setting progress { get => selfDatas.First(x => x.varName == "progress"); }
            public static Setting transit { get => selfDatas.First(x => x.varName == "transit"); }
            public static Setting completed { get => selfDatas.First(x => x.varName == "completed"); }
            public static Setting cancelled { get => selfDatas.First(x => x.varName == "cancelled"); }
            public static Setting returned { get => selfDatas.First(x => x.varName == "returned"); }
        }
        /// <summary>
        /// 付款方式代碼
        /// </summary>
        public static class paymentTypeCode
        {
            public static List<Setting> selfDatas { get => settingCodeDatas.Where(x => x.varKey == "paymentType").ToList(); }
            public static List<Setting> enableDatas { get => selfDatas.Where(x => x.varState == (int)varStateCode.enable).ToList(); }
            public static Setting COD { get => selfDatas.First(x => x.varName == "COD"); }
            public static Setting creditCard { get => selfDatas.First(x => x.varName == "creditCard"); }
        }
        /// <summary>
        /// 宅配方式代碼
        /// </summary>
        public static class deliveryTypeCode
        {
            public static List<Setting> selfDatas { get => settingCodeDatas.Where(x => x.varKey == "deliveryType").ToList(); }
            public static List<Setting> enableDatas { get => selfDatas.Where(x => x.varState == (int)varStateCode.enable).ToList(); }
            public static Setting homeDelivery { get => selfDatas.First(x => x.varName == "homeDelivery"); }
            public static Setting sevenEleven { get => selfDatas.First(x => x.varName == "sevenEleven"); }
            public static Setting familyMart { get => selfDatas.First(x => x.varName == "familyMart"); }
        }
        /// <summary>
        /// 人員權限代碼
        /// </summary>
        public static class memberRightCode
        {
            public static List<Setting> selfDatas { get => settingCodeDatas.Where(x => x.varKey == "memberRight").ToList(); }
            public static List<Setting> enableDatas { get => selfDatas.Where(x => x.varState == (int)varStateCode.enable).ToList(); }
            public static Setting admin { get => selfDatas.First(x => x.varName == "admin"); }
            public static Setting editor { get => selfDatas.First(x => x.varName == "editor"); }
            public static Setting user { get => selfDatas.First(x => x.varName == "user"); }
        }
        #endregion

        #region function
        /// <summary>
        /// 物件轉換型態(Newtonsoft.Json)
        /// </summary>
        public static T Mapping<T>(this object _data)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(_data));
        }
        /// <summary>
        /// 物件轉換型態(System.Text.Json.JsonSerializer)
        /// </summary>
        public static T MappingFromBody<T>(this object _data)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(System.Text.Json.JsonSerializer.Serialize(_data));
        }
        /// <summary>
        /// 資料初始化(string預設空字串、boolean預設false、數字預設0)
        /// </summary>
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
        /// <summary>
        /// 深層複製資料
        /// </summary>
        public static T CloneObj<T>(this T _data) where T : class
        {
            return _data.Mapping<T>();
        }
        /// <summary>
        /// 寫入LOG(Exception)
        /// </summary>
        public static void ToLog(this Exception _exception)
        {
            Log.Error(_exception.Message);
        }
        /// <summary>
        /// 寫入LOG(string)
        /// </summary>
        public static void ToLog(this string _message)
        {
            Log.Debug(_message);
        }
        /// <summary>
        /// 產生新Token
        /// </summary>
        public static string CreateToken(Member memberData)
        {
            var key = Encoding.ASCII.GetBytes(tokenKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var claimDatas = new List<Claim>
            {
                new Claim("memberId", Convert.ToString(memberData.memberId)),
                new Claim("email", Convert.ToString(memberData.email)),
            };
            var memberRightData = memberRightCode.enableDatas
                .FirstOrDefault(x => x.varValue == memberData.memberRight);
            if (memberRightData != null)
            {
                claimDatas.Add(new Claim(ClaimTypes.Role, memberRightData.varName));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claimDatas),
                Expires = DateTime.UtcNow.AddMinutes(tokenAddMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        /// <summary>
        /// DataTables動態搜尋
        /// </summary>
        public static IEnumerable<T> DynamicSearch<T>(this IEnumerable<T> data, string searchValue)
        {
            if (string.IsNullOrWhiteSpace(searchValue))
                return data;
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression condition = null;
            foreach (var property in typeof(T).GetProperties())
            {
                var left = Expression.Property(parameter, property);
                //數字
                if (property.PropertyType == typeof(int) || property.PropertyType == typeof(long) || property.PropertyType == typeof(double) || property.PropertyType == typeof(float))
                {
                    //轉字串
                    var toStringMethod = typeof(object).GetMethod("ToString", Type.EmptyTypes);
                    var toStringCall = Expression.Call(left, toStringMethod);
                    //模糊查詢
                    var right = Expression.Constant(searchValue);
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    var containsExpression = Expression.Call(toStringCall, containsMethod, right);
                    if (condition == null)
                        condition = containsExpression;
                    else
                        condition = Expression.OrElse(condition, containsExpression);
                }
                //字串
                else if (property.PropertyType == typeof(string))
                {
                    var isNullExpression = Expression.Equal(left, Expression.Constant(null, typeof(string)));
                    var containsExpression = Expression.Call(left, "Contains", null, Expression.Constant(searchValue));
                    //模糊查詢
                    var validCondition = Expression.AndAlso(Expression.Not(isNullExpression), containsExpression);
                    condition = condition == null ? validCondition : Expression.OrElse(condition, validCondition);
                }
            }
            if (condition != null)
            {
                var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
                return data.Where(lambda.Compile());
            }
            return data;
        }
        /// <summary>
        /// DataTables動態排序
        /// </summary>
        public static IEnumerable<T> DynamicSort<T>(this IEnumerable<T> data, string sortColumn = null, bool descending = false)
        {
            // 動態排序邏輯
            if (!String.IsNullOrEmpty(sortColumn))
            {
                var property = typeof(T).GetProperty(sortColumn);
                var parameter = Expression.Parameter(typeof(T), "x");
                if (property != null)
                {
                    var propertyExpression = Expression.Property(parameter, property);
                    var sortLambda = Expression.Lambda(propertyExpression, parameter);
                    var orderByMethod = descending
                        ? typeof(Enumerable).GetMethods().First(x => x.Name == "OrderByDescending" && x.GetParameters().Length == 2)
                        : typeof(Enumerable).GetMethods().First(x => x.Name == "OrderBy" && x.GetParameters().Length == 2);
                    var genericOrderBy = orderByMethod.MakeGenericMethod(typeof(T), property.PropertyType);
                    data = (IEnumerable<T>)genericOrderBy.Invoke(null, new object[] { data, sortLambda.Compile() });
                }
            }
            return data;
        }
        #endregion

    }
}
