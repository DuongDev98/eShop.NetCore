namespace eShop.ViewModels.Catalog.Carts
{
    public class CartPageVm
    {
        public List<CartVm> Carts { get; set; }
        public decimal Count { get; set; }
        public decimal Total { get; set; }
    }
}
