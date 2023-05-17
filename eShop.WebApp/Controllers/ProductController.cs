using Microsoft.AspNetCore.Mvc;

namespace eShop.WebApp.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Category(int id)
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            return View();
        }
    }
}
