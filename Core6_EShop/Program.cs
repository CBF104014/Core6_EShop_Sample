using Core6_EShop.Cls;
using Core6_EShop.Repository.Implement;
using Core6_EShop.Service.Implement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Security.Claims;
using System.Text;

/// 注意：
/// 寫入附件的資料權限須開啟
try
{
    var builder = WebApplication.CreateBuilder(args);
    //appsettings
    builder.Configuration
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
    //code init
    Code.InitCode(builder.Configuration);

    //log
    Log.Logger = new LoggerConfiguration()
        .ReadFrom
        .Configuration(builder.Configuration)
        .CreateLogger();
    builder.Host.UseSerilog();

    //repository
    builder.Services.AddScoped<SettingRepository>();
    builder.Services.AddScoped<MemberRepository>();
    builder.Services.AddScoped<GoodsRepository>();
    builder.Services.AddScoped<CartRepository>();
    builder.Services.AddScoped<GoodsRelationRepository>();
    builder.Services.AddScoped<GoodsSizeRepository>();
    builder.Services.AddScoped<CountryRepository>();
    builder.Services.AddScoped<OrderRepository>();
    builder.Services.AddScoped<OrderRelationRepository>();
    builder.Services.AddScoped<ArduinoA1Repository>();

    //services
    builder.Services.AddScoped<SettingService>();
    builder.Services.AddScoped<MemberService>();
    builder.Services.AddScoped<GoodsService>();
    builder.Services.AddScoped<CartService>();
    builder.Services.AddScoped<GoodsRelationService>();
    builder.Services.AddScoped<GoodsSizeService>();
    builder.Services.AddScoped<CountryService>();
    builder.Services.AddScoped<OrderService>();
    builder.Services.AddScoped<OrderRelationService>();
    builder.Services.AddScoped<ArduinoA1Service>();

    //jwt
    builder.Services
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                RoleClaimType = ClaimTypes.Role,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Code.tokenKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
            };
        });

    //request size limit
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.Limits.MaxRequestBodySize = Code.appMaxRequestBodySize;
    });
    builder.Services.AddControllersWithViews();
    //app build
    var app = builder.Build();
    app.UseExceptionHandler("/Home/Error");
    app.Use(async (context, next) =>
    {
        var jwtToken = context.Request.Cookies["jwtToken"];
        if (!String.IsNullOrEmpty(jwtToken) && !context.Request.Headers.Any(x => x.Key == "Authorization"))
            context.Request.Headers.Add("Authorization", "Bearer " + jwtToken);
        await next();
    });
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    app.Run();
}
catch (Exception ex)
{
    throw ex;
}

#region function
#endregion