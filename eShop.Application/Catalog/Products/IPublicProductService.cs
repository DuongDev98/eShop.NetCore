using eShop.ViewModels.Catalog.Products.Dtos;
using eShop.ViewModels.Dtos;

namespace eShop.Application.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PageResult<ProductViewModel>> GetAllByCategoryId(string langguageId, GetPublicProductPagingRequest request);
    }
}
