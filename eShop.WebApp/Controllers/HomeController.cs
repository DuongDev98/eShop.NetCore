using eShop.ApiIntegration.Product;
using eShop.ApiIntegration.Slide;
using eShop.Utilities.Contants;
using eShop.ViewModels.Utilities;
using eShop.WebApp.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;

namespace eShop.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ISlideApiClient _slideApiClient;
        private readonly IProductApiClient _productApiClient;
        public HomeController(IConfiguration configuration ,ISlideApiClient slideApiClient, IProductApiClient productApiClient)
        {
            _configuration = configuration;
            _slideApiClient = slideApiClient;
            _productApiClient = productApiClient;
        }

        public async Task<IActionResult> Index()
        {
            string baseUrl = _configuration.GetValue<string>(SystemConstants.AppSettings.BaseAddress);
            ViewData["baseUrl"] = baseUrl;

            var culture = CultureInfo.CurrentCulture.Name;
            var slideResult = await _slideApiClient.GetAll();
            var featureResult = await _productApiClient.GetListFeature(culture, 10);
            var latestResult = await _productApiClient.GetListLatest(culture, 20);
            var viewModel = new HomePageVm() {
                Slides = slideResult?.data,
                Features = featureResult?.data,
                Latests = latestResult?.data,
            };
            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SetCultureCookie(string cltr, string returnUrl)
        {
            string culture = CultureInfo.CurrentCulture.Name;
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return LocalRedirect(returnUrl.Replace(culture, cltr));
        }
    }
}