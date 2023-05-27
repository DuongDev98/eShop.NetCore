using eShop.Utilities.Contants;
using eShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using eShop.ApiIntegration.User;
using eShop.ApiIntegration.Role;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace eShop.WebApp.Controllers
{
	public class AccountController : Controller
	{
		private IConfiguration _configuration;
		private IUserApiClient _userClientApi;
		public AccountController(IConfiguration configuration, IUserApiClient userClientApi)
		{
			_configuration = configuration;
			_userClientApi = userClientApi;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginRequest request)
		{
			if (!ModelState.IsValid)
				return View();

			var result = await _userClientApi.Authenticate(request);

			if (!result.success || string.IsNullOrEmpty(result.data))
			{
				ModelState.AddModelError("", result.message);
				return View();
			}

			var userPrincipal = ValidateToken(result.data);
			var authProperties = new AuthenticationProperties()
			{
				ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
				IsPersistent = true
			};

			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties);

			HttpContext.Session.SetString(SystemConstants.AppSettings.Token, result.data);

			return RedirectToAction("Index", "Home");
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			HttpContext.Session.Remove(SystemConstants.AppSettings.Token);
			return RedirectToAction("Login", "Account");
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userClientApi.RegisterUser(request);

            if (!result.success)
            {
                ModelState.AddModelError("", result.message);
				return View();
			}

			TempData["successMessage"] = "Đăng ký tài khoản thàng công";

            return RedirectToAction("Login");
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
		{
			IdentityModelEventSource.ShowPII = true;

			SecurityToken validatedToken;
			TokenValidationParameters validationParameters = new TokenValidationParameters();

			validationParameters.ValidateLifetime = true;
			validationParameters.ValidAudience = _configuration.GetValue<string>("Tokens:Issuer");
			validationParameters.ValidIssuer = _configuration.GetValue<string>("Tokens:Issuer");
			validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Tokens:Key")));

			ClaimsPrincipal claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
			return claimsPrincipal;
		}
	}
}
