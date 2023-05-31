namespace eShop.ViewModels.Catalog.Orders
{
    public class OrderVm
    {
        public string Name { set; get; }
        public string Address { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public List<OrderDetailVm> OrderDetails { set; get; }
    }
}
