# asp.net core 6 E-SHOP 網站 
  
□ 框架：Asp.net core 6  
□ 語言：C#  
□ 資料庫：MySql  
□ 資料庫應用：Dapper  
□ 前端框架：Bootstrap5 + Vue3.js  
□ 其他技術及應用：JavaScript、CSS、Jquery、Json、Ajax、JWT  
#### ※需自行修改appsettings.json中的MySQLConnectionString  

    
## □架構  
[![架構](https://i.imgur.com/Um0huAz.png)](https://i.imgur.com/Um0huAz.png) 
  
  
## □首頁  
[![首頁](https://i.imgur.com/foCSJGc.png)](https://i.imgur.com/foCSJGc.png)  
  
## □登入頁面  
[![登入](https://i.imgur.com/gHFCzPs.png)](https://i.imgur.com/gHFCzPs.png)  
  
## □註冊頁面  
[![註冊](https://i.imgur.com/2u0NGKu.png)](https://i.imgur.com/2u0NGKu.png)  
[![註冊](https://i.imgur.com/lM9nxw9.png)](https://i.imgur.com/lM9nxw9.png)  
  
## □商品管理  
[![商品管理](https://i.imgur.com/1vNKtez.png)](https://i.imgur.com/1vNKtez.png)  
[![商品管理](https://i.imgur.com/KQ0T7O4.png)](https://i.imgur.com/KQ0T7O4.png)  
[![商品管理](https://i.imgur.com/5CLKD3J.png)](https://i.imgur.com/5CLKD3J.png)  
  
## □商品頁面  
[![商品](https://i.imgur.com/Nchiuyb.png)](https://i.imgur.com/Nchiuyb.png)  
[![商品](https://i.imgur.com/RZeUqzS.png)](https://i.imgur.com/RZeUqzS.png)  
  
## □購物車  
[![商品](https://i.imgur.com/Wsm6Ebx.png)](https://i.imgur.com/Wsm6Ebx.png)  
  
## □結帳  
[![商品](https://i.imgur.com/jWHlSEr.png)](https://i.imgur.com/jWHlSEr.png)  
  
## □帳戶(會員資料、購買紀錄)  
[![商品](https://i.imgur.com/2fAsb1G.png)](https://i.imgur.com/2fAsb1G.png)  
[![商品](https://i.imgur.com/BixSye9.png)](https://i.imgur.com/BixSye9.png)  

## □資料驗證相關程式碼  
```
/// <summary>
/// 人員
/// </summary>
public class Member
{
 public long rankey { get; set; }
 public int memberId { get; set; }
 [Required(ErrorMessage = "欄位必填")]
 public string email { get; set; }
 [Required(ErrorMessage = "欄位必填")]
 public string name { get; set; }
 public int zipCode { get; set; }
 public string countryCode { get; set; }
 public string city { get; set; }
 [Required(ErrorMessage = "欄位必填")]
 public string address { get; set; }
 [Required(ErrorMessage = "欄位必填")]
 public string phone { get; set; }
 public int memberState { get; set; }
 public int memberRight { get; set; }
}
```
```
/// <summary>
/// Controller 驗證
/// <summary>
[HttpPost]
public async Task<IActionResult> SaveRegisterData([FromBody] Member memberData)
{
 var validateData = ValidateModel();
 if (!validateData.isValid)
 {
 return Json(new APIDto((int)Code.stateCode.error, $"資料未填寫完整", "", new
 {
 validateData.validateDatas
 }));
 }
 var apiResult = await memberService.SaveData(memberData);
 return Json(apiResult);
}
```
```
/// <summary>
/// 驗證 Model
/// </summary>
public ValidateDto ValidateModel()
{
 return new ValidateDto()
 {
 isValid = ModelState.IsValid,
 validateDatas = ModelState.IsValid ? null : ModelState
 .Where(x => x.Value.Errors.Any())
 .Select(x => new ValidateDetailDto()
 {
 fieldFullName = x.Key,
 errorDatas = x.Value.Errors.Select(y => y.ErrorMessage)
 })
 };
}
```
