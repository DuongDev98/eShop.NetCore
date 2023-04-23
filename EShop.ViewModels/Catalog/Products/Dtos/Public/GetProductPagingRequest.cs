using eShop.ViewModels.Dtos;

namespace eShop.ViewModels.Catalog.Products.Dtos.Public
{
    public class GetProductPagingRequest : PagingRequestBase
    {
        public int categoryId { set; get; }
    }
}