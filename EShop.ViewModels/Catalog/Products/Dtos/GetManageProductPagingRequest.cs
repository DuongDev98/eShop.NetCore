using eShop.ViewModels.Dtos;

namespace eShop.ViewModels.Catalog.Products.Dtos
{
    public class GetManageProductPagingRequest : PagingRequestBase
    {
        public string keyword { set; get; }
        public List<int> categoryIds { set; get; }
    }
}