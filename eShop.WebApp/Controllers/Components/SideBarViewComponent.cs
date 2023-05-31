using eShop.ApiIntegration.Category;
using eShop.ApiIntegration.Product;
using eShop.ViewModels.Catalog.Carts;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

namespace eShop.WebApp.Controllers.Components
{
    public class SideBarViewComponent : ViewComponent
    {
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IProductApiClient _productApiClient;
        public SideBarViewComponent(ICategoryApiClient categoryApiClient, IProductApiClient productApiClient)
        {
            _categoryApiClient = categoryApiClient;
            _productApiClient = productApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string cuture = CultureInfo.CurrentCulture.Name;
            var categoryResult = await _categoryApiClient.GetAll(cuture);

            string languageId = CultureInfo.CurrentCulture.Name;
            string userID = Request.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var g = new Guid(userID ?? Guid.NewGuid().ToString());
            List<CartVm> lst = CartController.GetCarts(Request.HttpContext, g);
            int count = 0;
            decimal total = 0;
            foreach (CartVm c in lst)
            {
                var productResult = await _productApiClient.GetById(c.ProductId, languageId);
                count += c.Quantity;
                total += c.Quantity * c.Price;
            }
            CartPageVm model = new CartPageVm()
            {
                Carts = lst,
                Count = count,
                Total = total
            };
            ViewData["TotalItem"] = model.Count;
            ViewData["Total"] = model.Total;
            return View("Default", categoryResult.data);
        }
    }
}
