using eShop.ViewModels.Common;

namespace eShop.ViewModels.Catalog.Products
{
    public class GetProductRequest : PagingRequestBase
    {
        public string? keyword { set; get; }
        public int? categoryId { set; get; }
        public List<int>? categoryIds { set; get; }
        public string? languageId { set; get; }
    }
}