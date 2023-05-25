using eShop.ViewModels.Catalog.Products;

namespace eShop.ViewModels.Utilities
{
    public class HomePageVm
    {
        public List<SlideVm> Slides { set; get; }
        public List<ProductVm> Features { set; get; }
        public List<ProductVm> Latests { set; get; }
    }
}
