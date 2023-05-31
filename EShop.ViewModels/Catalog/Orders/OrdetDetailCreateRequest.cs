namespace eShop.ViewModels.Catalog.Orders
{
    public class OrderDetaiCreateRequest
    {
        public int ProductId { set; get; }
        public int Quantity { set; get; }
        public int Price { set; get; }
    }
}
