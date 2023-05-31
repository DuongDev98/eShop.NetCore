using eShop.ApiIntegration.Category;
using eShop.ApiIntegration.Order;
using eShop.ApiIntegration.Product;
using eShop.ApiIntegration.Slide;
using eShop.ApiIntegration.User;
using eShop.WebApp.LocalizationResources;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

var cultures = new[]
{
    new CultureInfo("vi"),
    new CultureInfo("en"),
};

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews()
    .AddExpressLocalization<ExpressLocalizationResource, ViewLocalizationResource>(
    ops =>
    {
        ops.ResourcesPath = "LocalizationResources";
        ops.RequestLocalizationOptions = o =>
        {
            o.SupportedCultures = cultures;
            o.SupportedUICultures = cultures;
            o.DefaultRequestCulture = new RequestCulture("vi");
        };
    });
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

//login
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options => options.LoginPath = "/vi/Account/Login");
builder.Services.Configure<IdentityOptions>(options => options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier);

//Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//declare DI
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ISlideApiClient, SlideApiClient>();
builder.Services.AddTransient<ICategoryApiClient, CategoryApiClient>();
builder.Services.AddTransient<IUserApiClient, UserApiClient>();
builder.Services.AddTransient<IProductApiClient, ProductApiClient>();
builder.Services.AddTransient<IOrderApiClient, OrderApiClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.UseRequestLocalization();

app.UseSession();

app.MapControllerRoute(name: "default", pattern: "{culture=vi}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(name: "category vn", pattern: "{culture}/danh-muc/{id}", new { controller="Product", action="Category" });
app.MapControllerRoute(name: "category en", pattern: "{culture}/categories/{id}", new { controller="Product", action="Category" });
app.MapControllerRoute(name: "product vn", pattern: "{culture}/san-pham/{id}", new { controller="Product", action="Detail" });
app.MapControllerRoute(name: "product en", pattern: "{culture}/products/{id}", new { controller="Product", action= "Detail" });

app.Run();
