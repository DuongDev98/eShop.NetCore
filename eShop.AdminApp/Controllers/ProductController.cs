using eShop.ApiIntegration.Category;
using eShop.ApiIntegration.Product;
using eShop.Utilities.Contants;
using eShop.ViewModels.Catalog.Categories;
using eShop.ViewModels.Catalog.Products.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eShop.AdminApp.Controllers
{
    public class ProductController : Controller
    {
        private IConfiguration _configuration;
        private ICategoryApiClient _categoryApiClient;
        private IProductApiClient _productApiClient;
        public ProductController(IConfiguration configuration, ICategoryApiClient categoryApiClient, IProductApiClient productApiClient)
        {
            _configuration = configuration;
            _categoryApiClient = categoryApiClient;
            _productApiClient = productApiClient;
        }

        public async Task<IActionResult> Index(GetProductRequest request)
        {
            ViewData["keyword"] = request.keyword;
            ViewData["pageIndex"] = request.pageIndex;
            ViewData["pageSize"] = request.pageSize;

            if (request.pageIndex == 0) request.pageIndex = 1;
            if (request.pageSize == 0) request.pageSize = 10;
            ViewData["Title"] = "Danh sách sản phẩm";
            List<CategoryVm> lst = new List<CategoryVm>();
            request.languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

            var resultCategories = await _categoryApiClient.GetAll(request.languageId);
            if (resultCategories.success) lst = resultCategories.data;
            ViewData["CategoriesSelectList"] = new SelectList(lst, "Id", "Name", request.categoryId);

            var result = await _productApiClient.GetPaging(request);
            if (TempData["successMessage"] != null)
            {
                ViewData["successMessage"] = TempData["successMessage"];
            }

            return View(result.data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Title"] = "Thêm mới sản phẩm";
            ViewData["SelectCategories"] = await GetSelectCategories();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateRequest request)
        {
            ViewData["Title"] = "Thêm mới sản phẩm";
            ViewData["SelectCategories"] = await GetSelectCategories();

            if (!ModelState.IsValid) return View();

            request.LanguageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var result = await _productApiClient.Create(request);

            if (result.success)
            {
                TempData["successMessage"] = "Thêm mới thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.message);

            return View();
        }

        private async Task<SelectList> GetSelectCategories()
        {
            var lst = new List<CategoryVm>();

            var result = await _categoryApiClient.GetAll(HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId));

            if (result.success) lst = result.data;

            return new SelectList(lst, "Id", "Name");
        }
    }
}