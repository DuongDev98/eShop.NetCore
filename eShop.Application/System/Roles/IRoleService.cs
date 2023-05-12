using eShop.ViewModels.Common;
using eShop.ViewModels.System.Roles;

namespace eShop.Application.System.Roles
{
    public interface IRoleService
    {
        Task<ApiResult<List<RoleVm>>> GetAll();
    }
}
