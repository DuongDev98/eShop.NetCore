using eShop.ViewModels.Dtos;

namespace eShop.ViewModels.Catalog.Products.Dtos.Manage
{
    public class GetProductPagingRequest : PagingRequestBase
    {
        public string keyword { set; get; }
        public List<int> categoryIds { set; get; }
    }
}