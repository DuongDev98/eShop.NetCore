﻿namespace eShop.ViewModels.Catalog.ProductImage
{
    public class ProductImageVm
    {
        public int Id { set; get; }
        public int ProductId { set; get; }
        public string ImagePath { set; get; }
        public string Caption { set; get; }
        public bool IsDefault { set; get; }
        public DateTime DateCreated { set; get; }
        public int SortOrder { set; get; }
        public long FileSize { set; get; }
    }
}