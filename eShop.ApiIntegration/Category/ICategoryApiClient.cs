using eShop.ViewModels.Catalog.Categories;
using eShop.ViewModels.Common;

namespace eShop.ApiIntegration.Category
{
    public interface ICategoryApiClient
    {
        Task<ApiResult<List<CategoryVm>>> GetAll(string languageId);
    }
}