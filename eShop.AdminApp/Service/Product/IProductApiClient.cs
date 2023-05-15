using eShop.ViewModels.Catalog.Products.Dtos;
using eShop.ViewModels.Common;

namespace eShop.AdminApp.Service.Product
{
    public interface IProductApiClient
    {
        Task<ApiResult<PagedResult<ProductVm>>> GetPaging(GetProductRequest request);
        Task<ApiResult<bool>> Create(ProductCreateRequest request);
    }
}
