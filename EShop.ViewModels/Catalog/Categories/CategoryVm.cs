namespace eShop.ViewModels.Catalog.Categories
{
    public class CategoryVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTitle { get; set; }
        public string SeoAlias { get; set; }
        public bool IsShowOnHome { get; set; }
        public int? ParentId { get; set; }
    }
}
