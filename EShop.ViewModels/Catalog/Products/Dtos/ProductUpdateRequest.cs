﻿using Microsoft.AspNetCore.Http;

namespace eShop.ViewModels.Catalog.Products.Dtos
{
    public class ProductUpdateRequest
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Details { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }
        public string SeoAlias { get; set; }
        public string? LanguageId { set; get; }
        public int CategoryId { set; get; }
        public bool IsFeatured { set; get; }
        public IFormFile? ThumbnailImage { set; get; }
    }
}
