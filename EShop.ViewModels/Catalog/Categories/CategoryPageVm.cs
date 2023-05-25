using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Common;

namespace eShop.ViewModels.Catalog.Categories
{
    public class CategoryPageVm
    {
        public CategoryPageVm()
        {
            PagedResultProduct = new PagedResult<ProductVm>();
        }

        public CategoryVm Category { get; set; }
        public PagedResult<ProductVm> PagedResultProduct { get; set; }
    }
}
