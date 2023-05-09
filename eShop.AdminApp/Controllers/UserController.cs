using eShop.AdminApp.Controllers;
using eShop.AdminApp.Service;
using eShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eshop.AdminApp.Controllers
{
    public class UserController : BaseController
    {
        private IUserClientApi _userClientApi;
        private IConfiguration _configuration;
        public UserController(IUserClientApi userClientApi, IConfiguration configuration)
        {
            _userClientApi = userClientApi;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(string keyword = "", int pageIndex = 1, int pageSize = 10)
        {
            var token = HttpContext.Session.GetString("Token");
            GetUsersPagingRequest request = new GetUsersPagingRequest()
            {
                BearerToken = token,
                keyword = keyword,
                pageIndex = pageIndex,
                pageSize = pageSize
            };
            var users = await _userClientApi.GetUsersPaging(request);
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(ModelState);

            var token = await _userClientApi.Authenticate(request);

            if (string.IsNullOrEmpty(token)) return View();

            var userPrincipal = ValidateToken(token);
            var authProperties = new AuthenticationProperties()
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties);

            HttpContext.Session.SetString("Token", token);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(RegisterRequest request)
        {
            if (!ModelState.IsValid) return View();

            var rs = await _userClientApi.RegisterUser(request);

            if (rs) return RedirectToAction("Index");

            return View();
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
