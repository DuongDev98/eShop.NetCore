using eShop.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;

namespace eShop.AdminApp.Controllers.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(PagedResultBase result)
        {
            return await Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
