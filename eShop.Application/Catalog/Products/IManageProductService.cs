using eShop.ViewModels.Catalog.Products.Dtos;
using eShop.ViewModels.Dtos;
using Microsoft.AspNetCore.Http;

namespace eShop.Application.Catalog.Products
{
    public interface IManageProductService
    {
        Task<int> Create(ProductCreateRequest request);
        Task<int> Update(ProductUpdateRequest request);
        Task<int> Delete(int productId);

        Task AddViewCount(int productId);
        Task UpdateStock(int productId, int addedQuantity);
        Task UpdatePrice(int productId, decimal newPrice);

        Task<PageResult<ProductViewModel>> getAllPaging(GetManageProductPagingRequest request);

        Task<int> AddImages(int productId, List<IFormFile> files);
        Task<int> RemoveImages(int imageId);
        Task<int> UpdateImage(int imageId, string caption, bool isDefault);
        Task<List<ProductImageViewModel>> GetListImage(int productId);
    }
}
