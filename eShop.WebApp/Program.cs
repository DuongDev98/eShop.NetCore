using eShop.ApiIntegration.Category;
using eShop.ApiIntegration.Product;
using eShop.ApiIntegration.Slide;
using eShop.WebApp.LocalizationResources;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

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

//

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
builder.Services.AddTransient<IProductApiClient, ProductApiClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

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
