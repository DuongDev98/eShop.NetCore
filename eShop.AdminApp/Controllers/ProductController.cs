using eShop.AdminApp.Service.Product;
using eShop.AdminApp.Service.Role;
using eShop.AdminApp.Service.User;
using eShop.Utilities.Contants;
using eShop.ViewModels.Catalog.Products.Dtos;
using eShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Mvc;

namespace eShop.AdminApp.Controllers
{
    public class ProductController : Controller
    {
        private IConfiguration _configuration;
        private IProductApiClient _productApiClient;
        public ProductController(IConfiguration configuration, IProductApiClient productApiClient)
        {
            _configuration = configuration;
            _productApiClient = productApiClient;
        }

        public async Task<IActionResult> Index(string keyword = "", int pageIndex = 1, int pageSize = 20)
        {
            string languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            GetProductRequest request = new GetProductRequest()
            {
                keyword = keyword,
                pageIndex = pageIndex,
                pageSize = pageSize,
                languageId = languageId
            };
            var result = await _productApiClient.GetPaging(request);

            ViewData["keyword"] = keyword;
            if (TempData["successMessage"] != null)
            {
                ViewData["successMessage"] = TempData["successMessage"];
            }

            return View(result.data);
        }
    }
}