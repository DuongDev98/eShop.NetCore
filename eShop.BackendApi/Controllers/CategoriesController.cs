using eShop.Application.Catalog.Categories;
using eShop.Application.System.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetAll(string languageId)
        {
            var roles = await _categoryService.GetAll(languageId);
            return Ok(roles);
        }
    }
}
