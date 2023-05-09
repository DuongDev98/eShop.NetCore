using eShop.Data.Entities;
using eShop.ViewModels.Common;
using eShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eShop.Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;
        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;   
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<string> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return string.Empty;

            var result = await _signInManager.PasswordSignInAsync(user, request.PassWord, request.RememberMe, true);
            if (!result.Succeeded)
            {
                return string.Empty;
            }

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(";", roles))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Tokens:Key")));
            var cres = new SigningCredentials(key , SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer: _configuration.GetValue<string>("Tokens:Issuer"),
                    audience: _configuration.GetValue<string>("Tokens:Issuer"),
                    expires: DateTime.Now.AddHours(3),
                    claims: claims,
                    signingCredentials: cres);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<PagedResult<UserVm>> GetUsersPaging(GetUsersPagingRequest request)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(request.keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.keyword) || x.PhoneNumber.Contains(request.keyword));
            }

            int totalCount = await query.CountAsync();

            var data = query.Skip((request.pageIndex - 1) * request.pageSize).Take(request.pageSize)
                .Select(x=> new UserVm()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                    Email = x.PhoneNumber,
                }).ToList();

            var pagedResult = new PagedResult<UserVm>()
            {
                Items = data,
                TotalRecord = totalCount
            };

            return pagedResult;
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            var user = new AppUser()
            {
                Dob = request.Dob,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
            };

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded) return true;
            else return false;
        }
    }
}