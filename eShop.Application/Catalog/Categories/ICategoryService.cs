using eShop.ViewModels.Catalog.Categories;
using eShop.ViewModels.Common;

namespace eShop.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        Task<ApiResult<List<CategoryVm>>> GetAll(string languageId);
    }
}
