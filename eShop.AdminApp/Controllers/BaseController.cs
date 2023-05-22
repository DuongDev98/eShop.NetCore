using eShop.Utilities.Contants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eShop.AdminApp.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerName = ((ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
            var actionName = ((ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            if (!(controllerName == "User" && (actionName == "Login" || actionName == "Register")))
            {
                var token = context.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
                if (token == null)
                {
                    context.Result = new RedirectToActionResult("Login", "User", null);
                }
            }
        }
    }
}
