using eShop.ViewModels.Catalog.Products.Dtos;
using eShop.ViewModels.Catalog.Products.Dtos.Public;
using eShop.ViewModels.Dtos;

namespace eShop.Application.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PageResult<ProductViewModel>> GetAllByCategoryId(GetProductPagingRequest request);
    }
}
