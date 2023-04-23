namespace eShop.ViewModels.Catalog.Products.Dtos
{
    public class ProductImageViewModel
    {
        public string ImagePath { set; get; }
        public string Caption { set; get; }
        public bool IsDefault { set; get; }
        public DateTime DateCreated { set; get; }
        public int SortOrder { set; get; }
        public long FileSize { set; get; }
    }
}