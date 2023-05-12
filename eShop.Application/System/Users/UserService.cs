using Azure.Core;
using eShop.Data.Entities;
using eShop.ViewModels.Common;
using eShop.ViewModels.System.Roles;
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
        private readonly IConfiguration _configuration;
        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;   
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return new ApiErrorResult<string>("Tài khoản không tồn tại");

            var result = await _signInManager.PasswordSignInAsync(user, request.PassWord, request.RememberMe, true);
            if (!result.Succeeded)
            {
                return new ApiErrorResult<string>("Mật khẩu đăng nhập không chính xác");
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

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new ApiSuccessResult<string>(tokenString);
        }

        public async Task<ApiResult<PagedResult<UserVm>>> GetUsersPaging(GetUsersPagingRequest request)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(request.keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.keyword) || x.PhoneNumber.Contains(request.keyword));
            }

            int totalCount = await query.CountAsync();

            query = query.OrderBy(x => x.UserName);
            var data = query.Skip((request.pageIndex - 1) * request.pageSize).Take(request.pageSize)
                .Select(x=> new UserVm()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                    Email = x.Email,
                }).ToList();

            var pagedResult = new PagedResult<UserVm>()
            {
                Items = data,
                PageIndex = request.pageIndex,
                PageSize = request.pageSize,
                TotalRecords = totalCount
            };

            return new ApiSuccessResult<PagedResult<UserVm>>(pagedResult);
        }

        public async Task<ApiResult<bool>> Register(RegisterRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null) return new ApiErrorResult<bool>("Tài khoản đã tồn tại");

            user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null) return new ApiErrorResult<bool>("Email đã tồn tại");

            user = new AppUser()
            {
                Dob = request.Dob,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
            };

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded) return new ApiSuccessResult<bool>();
            else return new ApiErrorResult<bool>("Đăng ký không thành công");
        }

        public async Task<ApiResult<bool>> Update(UserUpdateRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null) return new ApiErrorResult<bool>("Tài khoản không tồn tại");

            //kiểm tra email đã tồn tại chưa
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Email đã tồn tại");
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Dob = request.Dob;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return new ApiSuccessResult<bool>();
            else return new ApiErrorResult<bool>("Cập nhật dữ liệu thành công");
        }

        public async Task<ApiResult<UserVm>> GetById(Guid Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<UserVm>("Người dùng không tồn tại trong hệ thống");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return new ApiSuccessResult<UserVm>(new UserVm()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Dob = user.Dob,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                Roles = (List<string>)roles
            });
        }

        public async Task<ApiResult<bool>> Delete(Guid Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Người dùng không tồn tại trong hệ thống");
            }

            IdentityResult result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            else
            {
                return new ApiErrorResult<bool>("Có lỗi trong quá trình xóa người dùng");
            }
        }

        public async Task<ApiResult<bool>> RoleAssign(Guid Id, RoleAssignRequest request)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Người dùng không tồn tại trong hệ thống");
            }

            var remove = request.Roles.Where(x => !x.Selected).Select(x => x.Name).ToList();
            foreach (string roleName in remove)
            {
                bool exists = await _userManager.IsInRoleAsync(user, roleName);
                if (exists) await _userManager.RemoveFromRoleAsync(user, roleName);
            }

            var added = request.Roles.Where(x => x.Selected).Select(x => x.Name).ToList();
            foreach (string roleName in added)
            {
                bool exists = await _userManager.IsInRoleAsync(user, roleName);
                if (!exists) await _userManager.AddToRoleAsync(user, roleName);
            }

            return new ApiSuccessResult<bool>();
        }
    }
}