using eShop.ViewModels.Common;
using eShop.ViewModels.System.Roles;

namespace eShop.AdminApp.Service.Role
{
    public interface IRoleApiClient
    {
        Task<ApiResult<List<RoleVm>>> GetAll();
    }
}
