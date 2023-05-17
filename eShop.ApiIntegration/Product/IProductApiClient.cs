using eShop.ViewModels.Catalog.Products.Dtos;
using eShop.ViewModels.Common;

namespace eShop.ApiIntegration.Product
{
    public interface IProductApiClient
    {
        Task<ApiResult<PagedResult<ProductVm>>> GetPaging(GetProductRequest request);
        Task<ApiResult<bool>> Create(ProductCreateRequest request);
        Task<ApiResult<List<ProductVm>>> GetListFeature(string languageId, int take);
        Task<ApiResult<List<ProductVm>>> GetListLatest(string languageId, int take);
    }
}
