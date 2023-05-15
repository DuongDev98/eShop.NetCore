using Microsoft.AspNetCore.Http;

namespace eShop.ViewModels.Catalog.Products.Dtos
{
    public class ProductCreateRequest
    {
        public string Name { set; get; }
        public decimal Price { set; get; }
        public decimal OriginalPrice { set; get; }
        public int Stock { set; get; }
        public string Description { set; get; }
        public string Details { set; get; }
        public string SeoAlias { get; set; }
        public string SeoTitle { set; get; }
        public string SeoDescription { set; get; }
        public string? LanguageId { set; get; }
        public int CategoryId { set; get; }
        public IFormFile ThumbnailImage { set; get; }
    }
}