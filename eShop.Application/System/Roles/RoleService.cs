using eShop.Data.Entities;
using eShop.ViewModels.Common;
using eShop.ViewModels.System.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eShop.Application.System.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ApiResult<List<RoleVm>>> GetAll()
        {
            var roles = await _roleManager.Roles.Select(x=>
            new RoleVm() {
                Id = x.Id.ToString(),
                Name = x.Name,
                Description = x.Description
            }).ToListAsync();

            return new ApiSuccessResult<List<RoleVm>>(roles);
        }
    }
}
