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

namespace eShop.AdminApp.Controllers
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

        public async Task<IActionResult> Index(string keyword = "", int pageIndex = 1, int pageSize = 20)
        {
            GetUsersPagingRequest request = new GetUsersPagingRequest()
            {
                keyword = keyword,
                pageIndex = pageIndex,
                pageSize = pageSize
            };
            var result = await _userClientApi.GetUsersPaging(request);
            return View(result.data);
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

            var result = await _userClientApi.Authenticate(request);

            if (!result.success || string.IsNullOrEmpty(result.data)) return View();

            var userPrincipal = ValidateToken(result.data);
            var authProperties = new AuthenticationProperties()
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties);

            HttpContext.Session.SetString("Token", result.data);

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
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid) return View();

            var result = await _userClientApi.RegisterUser(request);

            if (result.success) return RedirectToAction("Index");

            ModelState.AddModelError("", result.message);

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _userClientApi.GetUserById(id);
            if (!result.success)
            {
                return BadRequest(result);
            }
            //chuyển từ UserVm => UpdateUserRequet
            UpdateUserRequest model = new UpdateUserRequest()
            {
                Id = result.data.Id,
                FirstName = result.data.FirstName,
                LastName = result.data.LastName,
                Email = result.data.Email,
                Dob = result.data.Dob,
                PhoneNumber = result.data.PhoneNumber
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserRequest request)
        {
            if (!ModelState.IsValid) return View();

            var result = await _userClientApi.UpdateUser(request);

            if (result.success) return RedirectToAction("Index");

            ModelState.AddModelError("", result.message);

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _userClientApi.GetUserById(id);
            if (!result.success)
            {
                return BadRequest(result);
            }
            return View(result.data);
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
