using System.ComponentModel;

namespace eShop.ViewModels.Catalog.Orders
{
    public class OrderCreateRequest
    {
        public Guid UserId {set; get;}
        [DisplayName("Tên người nhận")]
        public string Name {set; get; }
        [DisplayName("Địa chỉ giao hàng")]
        public string Address { set; get; }
        public string Email {set; get;}
        [DisplayName("Điện thoại")]
        public string Phone {set; get;}
        public List<OrderDetaiCreateRequest> OrderDetais { set; get;}
    }
}
