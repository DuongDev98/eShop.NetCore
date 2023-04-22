using eShop.Application.Catalog.Products.Dtos;
using eShop.Application.Catalog.Products.Dtos.Manage;
using eShop.Application.Dtos;

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

        Task<PageResult<ProductViewModel>> getAll();
        Task<PageResult<ProductViewModel>> getAllPaging(GetProductPagingRequest request);
    }
}
