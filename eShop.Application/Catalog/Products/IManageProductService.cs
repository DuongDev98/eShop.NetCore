using eShop.Data.Entities;
using eShop.ViewModels.Catalog.ProductImage;
using eShop.ViewModels.Catalog.Products.Dtos;
using eShop.ViewModels.Dtos;
using Microsoft.AspNetCore.Http;

namespace eShop.Application.Catalog.Products
{
    public interface IManageProductService
    {
        Task<ProductViewModel> GetById(int productId, string languageId);
        Task<int> Create(ProductCreateRequest request);
        Task<int> Update(ProductUpdateRequest request);
        Task<int> Delete(int productId);

        Task<int> AddViewCount(int productId);
        Task<int> UpdateStock(int productId, int addedQuantity);
        Task<int> UpdatePrice(int productId, decimal newPrice);

        Task<PageResult<ProductViewModel>> getAllPaging(string languageId, GetManageProductPagingRequest request);

        Task<int> AddImage(int productId, ProductImageCreateRequest request);
        Task<int> RemoveImage(int imageId);
        Task<int> UpdateImage(int imageId, ProductImageCreateRequest request);
        Task<List<ProductImageViewModel>> GetListImages(int productId);
        Task<ProductImageViewModel> GetImageById(int imageId);
    }
}
