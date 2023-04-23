using eShop.ViewModels.Dtos;

namespace eShop.ViewModels.Catalog.Products.Dtos
{
    public class GetPublicProductPagingRequest : PagingRequestBase
    {
        public int categoryId { set; get; }
    }
}