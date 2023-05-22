using eShop.ApiIntegration.Product;
using eShop.ViewModels.Catalog.Products.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace eShop.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        public ProductController(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Category(int id, GetProductRequest request)
        {
            request.categoryId = id;
            request.languageId = CultureInfo.CurrentCulture.Name;
            if (request.pageIndex == 0) request.pageIndex = 1;
            if (request.pageSize == 0) request.pageSize = 10;
            var productResult = await _productApiClient.GetAll(request);
            return View(productResult);
        }

        public IActionResult Detail(int id)
        {
            return View();
        }
    }
}