using eShop.ApiIntegration.Order;
using eShop.ApiIntegration.Product;
using eShop.ViewModels.Catalog.Orders;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

namespace eShop.WebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IProductApiClient _productApiClient;
        private readonly IOrderApiClient _orderApiClient;
        public OrderController(IConfiguration configuration, IOrderApiClient orderApiClient, IProductApiClient productApiClient)
        {
            _configuration = configuration;
            _productApiClient = productApiClient;
            _orderApiClient = orderApiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["details"] = await GetInfo();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                var orderResult = await _orderApiClient.Create(request);
                if (orderResult.success)
                {
                    TempData["successMessage"] = "Tạo đơn hàng thành công";
                    //clear data gio hang
                    CartController.ClearCart(new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)), HttpContext);
                    return RedirectToAction("Index", "Cart");
                }
                else
                {
                    ModelState.AddModelError("", orderResult.message);
                }
            }
            ViewData["details"] = await GetInfo();
            return View();
        }

        public async Task<List<OrderDetailVm>> GetInfo()
        {
            List<OrderDetailVm> lstVm = new List<OrderDetailVm>();
            var carts = CartController.GetCarts(HttpContext, new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            foreach (var c in carts)
            {
                var productResult = await _productApiClient.GetById(c.ProductId, CultureInfo.CurrentCulture.Name);

                OrderDetailVm vm = new OrderDetailVm();
                vm.ProductId = c.ProductId;
                vm.Name = productResult.data.Name;
                vm.Price = c.Price;
                vm.Quantity = c.Quantity;
                lstVm.Add(vm);
            }
            return lstVm;
        }
    }
}
