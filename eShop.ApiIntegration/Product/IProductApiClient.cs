using eShop.ViewModels.Catalog.Products.Dtos;
using eShop.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;

namespace eShop.ApiIntegration.Product
{
    public interface IProductApiClient
    {
        Task<ApiResult<ProductVm>> GetById(int productId, string languageId);
        Task<ApiResult<PagedResult<ProductVm>>> GetAll(GetProductRequest request);
        Task<ApiResult<bool>> Create(ProductCreateRequest request);
        Task<ApiResult<bool>> Update(ProductUpdateRequest request);
        Task<ApiResult<bool>> Delete(ProductDeleteRequest request);
        Task<ApiResult<List<ProductVm>>> GetListFeature(string languageId, int take);
        Task<ApiResult<List<ProductVm>>> GetListLatest(string languageId, int take);
    }
}
