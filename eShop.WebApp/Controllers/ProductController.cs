using eShop.ApiIntegration.Category;
using eShop.ApiIntegration.Product;
using eShop.Utilities.Contants;
using eShop.ViewModels.Catalog.Categories;
using eShop.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace eShop.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        public ProductController(IConfiguration configuration, IProductApiClient productApiClient, ICategoryApiClient categoryApiClient)
        {
            _configuration = configuration;
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Category(int id, GetProductRequest request)
        {
            ViewData["baseUrl"] = _configuration.GetValue<string>(SystemConstants.AppSettings.BaseAddress);

            request.categoryId = id;
            request.languageId = CultureInfo.CurrentCulture.Name;
            if (request.pageIndex == 0) request.pageIndex = 1;
            if (request.pageSize == 0) request.pageSize = 10;
            var productResult = await _productApiClient.GetAll(request);
            if (!productResult.success) return BadRequest(productResult.message);

            var categoryResult = await _categoryApiClient.GetById(id, request.languageId);
            var model = new CategoryPageVm() {
                Category = categoryResult.data,
                PagedResultProduct = productResult.data
            };

            return View(model);
        }

        public async Task<IActionResult> Detail(int id)
        {
            ViewData["baseUrl"] = _configuration.GetValue<string>(SystemConstants.AppSettings.BaseAddress);

            var productResult = await _productApiClient.GetById(id, CultureInfo.CurrentCulture.Name);
            if (!productResult.success) return BadRequest(productResult.message);

            var categoryResult = await _categoryApiClient.GetById(productResult.data.CategoryId, CultureInfo.CurrentCulture.Name);
            if (!categoryResult.success) return BadRequest(categoryResult.message);

            var model = new ProductDetailVm()
            {
                Category = categoryResult.data,
                Product = productResult.data
            };

            return View(model);
        }
    }
}