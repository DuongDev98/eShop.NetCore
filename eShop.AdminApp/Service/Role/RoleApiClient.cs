using eShop.AdminApp.Service.Common;
using eShop.ViewModels.Common;
using eShop.ViewModels.System.Roles;

namespace eShop.AdminApp.Service.Role
{
    public class RoleApiClient : BaseApiClient, IRoleApiClient
    {
        public RoleApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor) {}

        public async Task<ApiResult<List<RoleVm>>> GetAll()
        {
            return await GetAsync<List<RoleVm>>("roles");
        }
    }
}
