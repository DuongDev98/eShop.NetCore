using eShop.AdminApp.Service.Role;
using eShop.AdminApp.Service.User;
using eShop.Utilities.Contants;
using eShop.ViewModels.Common;
using eShop.ViewModels.System.Roles;
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
        private IConfiguration _configuration;
        private IUserApiClient _userClientApi;
        private IRoleApiClient _roleClientApi;
        public UserController(IConfiguration configuration, IUserApiClient userClientApi, IRoleApiClient roleClientApi)
        {
            _configuration = configuration;
            _userClientApi = userClientApi;
            _roleClientApi = roleClientApi;
        }

        public async Task<IActionResult> Index(string keyword = "", int pageIndex = 1, int pageSize = 20)
        {
            GetUsersRequest request = new GetUsersRequest()
            {
                keyword = keyword,
                pageIndex = pageIndex,
                pageSize = pageSize
            };
            var result = await _userClientApi.GetAll(request);

            ViewData["keyword"] = keyword;
            if (TempData["successMessage"] != null)
            {
                ViewData["successMessage"] = TempData["successMessage"];
            }

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
            HttpContext.Session.SetString(SystemConstants.AppSettings.DefaultLanguageId, "vi-VN");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove(SystemConstants.AppSettings.Token);
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

            if (result.success)
            {
                TempData["successMessage"] = "Thêm mới thành công";
                return RedirectToAction("Index");
            }

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
            UserUpdateRequest model = new UserUpdateRequest()
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
        public async Task<IActionResult> Edit(UserUpdateRequest request)
        {
            if (!ModelState.IsValid) return View();

            var result = await _userClientApi.UpdateUser(request);

            if (result.success)
            {
                TempData["successMessage"] = "Cập nhật thành công";
                return RedirectToAction("Index");
            }

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

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userClientApi.GetUserById(id);
            if (!result.success)
            {
                return BadRequest(result);
            }
            return View(result.data);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            if (!ModelState.IsValid) return View();

            var result = await _userClientApi.DeleteUser(request.Id);

            if (result.success)
            {
                TempData["successMessage"] = "Xóa người dùng thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.message);

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RoleAssign(Guid id)
        {
            RoleAssignRequest roleAssign = await GetRoleAssignRequest(id);
            return View(roleAssign);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userClientApi.RoleAssign(request.Id, request);

            if (result.success)
            {
                TempData["successMessage"] = "Cập nhật quyền thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.message);

            RoleAssignRequest roleAssign = await GetRoleAssignRequest(request.Id);
            return View(roleAssign);
        }

        private async Task<RoleAssignRequest> GetRoleAssignRequest(Guid id)
        {
            var userObj = await _userClientApi.GetUserById(id);
            var roleObj = await _roleClientApi.GetAll();
            var roleAssignRequest = new RoleAssignRequest();
            foreach (var role in roleObj.data)
            {
                roleAssignRequest.Roles.Add(new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Description = role.Description,
                    Selected = userObj.data.Roles.Contains(role.Name)
                });
            }
            return roleAssignRequest;
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
