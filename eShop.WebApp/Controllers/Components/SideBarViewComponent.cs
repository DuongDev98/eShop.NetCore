using eShop.ApiIntegration.Category;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace eShop.WebApp.Controllers.Components
{
    public class SideBarViewComponent : ViewComponent
    {
        private readonly ICategoryApiClient _categoryApiClient;
        public SideBarViewComponent(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string cuture = CultureInfo.CurrentCulture.Name;
            var categoryResult = await _categoryApiClient.GetAll(cuture);
            return View("Default", categoryResult.data);
        }
    }
}
