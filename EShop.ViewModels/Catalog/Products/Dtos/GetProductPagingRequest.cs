using eShop.ViewModels.Common;

namespace eShop.ViewModels.Catalog.Products.Dtos
{
    public class GetProductPagingRequest : PagingRequestBase
    {
        public string keyword { set; get; }
        public int categoryId { set; get; }
        public List<int> categoryIds { set; get; }
    }
}