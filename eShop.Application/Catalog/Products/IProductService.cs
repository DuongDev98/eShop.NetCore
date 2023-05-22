using eShop.ViewModels.Catalog.ProductImage;
using eShop.ViewModels.Catalog.Products.Dtos;
using eShop.ViewModels.Common;

namespace eShop.Application.Catalog.Products
{
    public interface IProductService
    {
        Task<ApiResult<ProductVm>> GetById(int productId, string languageId);
        Task<ApiResult<int>> Create(ProductCreateRequest request);
        Task<ApiResult<int>> Update(ProductUpdateRequest request);
        Task<ApiResult<int>> Delete(int productId);

        Task<ApiResult<int>> AddViewCount(int productId);
        Task<ApiResult<int>> UpdateStock(int productId, int addedQuantity);
        Task<ApiResult<int>> UpdatePrice(int productId, decimal newPrice);

        Task<ApiResult<int>> AddImage(int productId, ProductImageCreateRequest request);
        Task<ApiResult<int>> RemoveImage(int imageId);
        Task<ApiResult<int>> UpdateImage(int imageId, ProductImageCreateRequest request);
        Task<ApiResult<List<ProductImageVm>>> GetListImages(int productId);
        Task<ApiResult<ProductImageVm>> GetImageById(int imageId);

        Task<ApiResult<PagedResult<ProductVm>>> GetAll(GetProductRequest request);
        Task<ApiResult<List<ProductVm>>> GetListFeature(string languageId, int take);
        Task<ApiResult<List<ProductVm>>> GetListLatest(string languageId, int take);
    }
}
