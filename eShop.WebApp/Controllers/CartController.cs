using eShop.ApiIntegration.Product;
using eShop.Utilities.Contants;
using eShop.ViewModels.Catalog.Carts;
using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Security.Claims;

namespace eShop.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IProductApiClient _productApiClient;
        public CartController(IConfiguration configuration, IProductApiClient productApiClient)
        {
            _configuration = configuration;
            _productApiClient = productApiClient;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["BaseAddress"] = _configuration.GetValue<string>(SystemConstants.AppSettings.BaseAddress);
            string languageId = CultureInfo.CurrentCulture.Name;
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid g = new Guid(userID ?? Guid.NewGuid().ToString());
            List<CartVm> lst = GetCarts(Request.HttpContext, g);
            int count = 0;
            decimal total = 0;
            foreach (CartVm c in lst)
            {
                var productResult = await _productApiClient.GetById(c.ProductId, languageId);
                count += c.Quantity;
                total += c.Quantity * c.Price;
                c.ProductName = productResult.data.Name;
                c.ThumbnailImage = productResult.data.ThumbnailImage;
            }
            CartPageVm model = new CartPageVm()
            {
                Carts = lst,
                Count = count,
                Total = total
            };

            if (TempData["successMessage"] != null)
            {
                ViewData["successMessage"] = TempData["successMessage"];
            }

            return View(model);
        }

        public async Task<IActionResult> Them(Guid userId, int productId)
        {
            bool added = false;
            List<CartVm> carts = GetCarts(HttpContext);
            foreach (CartVm cart in carts)
            {
                if (cart.UserId == userId && cart.ProductId == productId)
                {
                    cart.Quantity++;
                    added = true;
                }
            }

            if (!added)
            {
                ApiResult<ProductVm> productResult = await _productApiClient.GetById(productId, CultureInfo.CurrentCulture.Name);
                CartVm model = new CartVm() {
                    UserId = userId,
                    ProductId = productId,
                    Price = (int)productResult.data.Price,
                    Quantity = 1
                };
                carts.Add(model);
            }

            SaveCart(carts, HttpContext);
            return Content("ok");
        }

        public IActionResult Tang(Guid userId, int productId)
        {
            List<CartVm> carts = GetCarts(HttpContext);
            foreach (CartVm cart in carts)
            {
                if (cart.UserId == userId && cart.ProductId == productId)
                {
                    cart.Quantity++;
                }
            }
            SaveCart(carts, HttpContext);
            return Content("ok");
        }

        public IActionResult Giam(Guid userId, int productId)
        {
            List<CartVm> carts = GetCarts(HttpContext);
            foreach (CartVm cart in carts)
            {
                if (cart.UserId == userId && cart.ProductId == productId && cart.Quantity > 0)
                {
                    cart.Quantity--;
                }
            }
            SaveCart(carts, HttpContext);
            return Content("ok");
        }

        public IActionResult Xoa(Guid userId, int productId)
        {
            int index = -1;
            List<CartVm> carts = GetCarts(HttpContext);
            foreach (CartVm cart in carts)
            {
                if (cart.UserId == userId && cart.ProductId == productId)
                {
                    index = carts.IndexOf(cart);
                }
            }
            if (index >= 0)
            {
                carts.RemoveAt(index);
                SaveCart(carts, HttpContext);
            }
            return Content("ok");
        }

        public async Task<IActionResult> DoiSL(Guid userId, int productId, int quantity)
        {
            bool added = false;
            List<CartVm> carts = GetCarts(HttpContext);
            foreach (CartVm cart in carts)
            {
                if (cart.UserId == userId && cart.ProductId == productId)
                {
                    added = true;
                    cart.Quantity = quantity;
                }
            }

            if (!added)
            {
                ApiResult<ProductVm> productResult = await _productApiClient.GetById(productId, CultureInfo.CurrentCulture.Name);
                CartVm model = new CartVm()
                {
                    UserId = userId,
                    ProductId = productId,
                    Price = (int)productResult.data.Price,
                    Quantity = 1
                };
                carts.Add(model);
            }

            SaveCart(carts, HttpContext);
            return Content("ok");
        }

        public static void ClearCart(Guid userId, HttpContext httpContext)
        {
            List<CartVm> carts = GetCarts(httpContext);
            for (int i = carts.Count - 1; i >= 0; i--)
            {
                if (carts[i].UserId == userId) carts.RemoveAt(i);
            }
            SaveCart(carts, httpContext);
        }

        private static void SaveCart(List<CartVm> carts, HttpContext httpContext)
        {
            string json = JsonConvert.SerializeObject(carts);
            httpContext.Session.SetString(SystemConstants.WebSettings.CartName, json);
        }

        public static List<CartVm> GetCarts(HttpContext context, Guid userId)
        {
            List<CartVm> results = new List<CartVm>();
            List<CartVm> tmp = GetCarts(context);
            foreach (CartVm c in tmp)
            {
                if (c.UserId == userId) results.Add(c);
            }
            return results;
        }

        public static List<CartVm> GetCarts(HttpContext context)
        {
            string json = context.Session.GetString(SystemConstants.WebSettings.CartName);
            if (json == null || json.Length == 0) return new List<CartVm>();
            else return JsonConvert.DeserializeObject<List<CartVm>>(json);
        }
    }
}