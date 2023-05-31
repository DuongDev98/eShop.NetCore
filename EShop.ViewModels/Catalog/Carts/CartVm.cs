namespace eShop.ViewModels.Catalog.Carts
{
    public class CartVm
    {
        public int ProductId { set; get; }
        public int Quantity { set; get; }
        public int Price { set; get; }

        public Guid UserId { get; set; }
        public string ProductName { set; get; }
        public string ThumbnailImage { set; get; }
    }
}
