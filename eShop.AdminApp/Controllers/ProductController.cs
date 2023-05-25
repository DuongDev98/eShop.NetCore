using eShop.ApiIntegration.Category;
using eShop.ApiIntegration.Product;
using eShop.Utilities.Contants;
using eShop.ViewModels.Catalog.Categories;
using eShop.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eShop.AdminApp.Controllers
{
    //[Authorize]
    public class ProductController : BaseController
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

            var result = await _productApiClient.GetAll(request);
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
            ViewData["SelectCategories"] = await GetSelectCategories(null);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateRequest request)
        {
            ViewData["Title"] = "Thêm mới sản phẩm";
            ViewData["SelectCategories"] = await GetSelectCategories(request.CategoryId);

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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Chỉnh sửa sản phẩm";
            var result = await _productApiClient.GetById(id, HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId));
            if (!result.success)
            {
                return BadRequest(result);
            }
            //chuyển từ UserVm => UpdateUserRequet
            ProductUpdateRequest model = new ProductUpdateRequest()
            {
                Id = result.data.Id,
                Name = result.data.Name,
                Description = result.data.Description,
                Details = result.data.Details,
                SeoAlias = result.data.SeoAlias,
                SeoDescription = result.data.SeoDescription,
                SeoTitle = result.data.SeoTitle,
                IsFeatured = result.data.IsFeatured,
                CategoryId = result.data.CategoryId,
            };
            ViewData["SelectCategories"] = await GetSelectCategories(model.CategoryId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductUpdateRequest request)
        {
            ViewData["Title"] = "Chỉnh sửa sản phẩm";
            ViewData["SelectCategories"] = await GetSelectCategories(request?.CategoryId);

            if (!ModelState.IsValid) return View();

            request.LanguageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var result = await _productApiClient.Update(request);

            if (result.success)
            {
                TempData["successMessage"] = "Thêm mới thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.message);

            return View();
        }

        private async Task<SelectList> GetSelectCategories(int? categoryId)
        {
            var lst = new List<CategoryVm>();

            var result = await _categoryApiClient.GetAll(HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId));

            if (result.success) lst = result.data;

            return new SelectList(lst, "Id", "Name", categoryId);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ViewData["Title"] = "Chỉnh sửa sản phẩm";
            var result = await _productApiClient.GetById(id, HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId));
            if (!result.success)
            {
                return BadRequest(result);
            }
            return View(result.data);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductDeleteRequest request)
        {
            ViewData["Title"] = "Xóa sản phẩm";
            if (!ModelState.IsValid) return View();

            var result = await _productApiClient.Delete(request);

            if (result.success)
            {
                TempData["successMessage"] = "Xóa sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.message);

            return View();
        }

        public async Task<IActionResult> Fake()
        {
            await _productApiClient.Fake();
            return Content("ok");
        }
    }
}