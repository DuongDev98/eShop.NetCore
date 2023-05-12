using eShop.AdminApp.Service.Common;
using eShop.ViewModels.Common;
using eShop.ViewModels.System.Roles;
using eShop.ViewModels.System.Users;
using Newtonsoft.Json;
using System.Net.Http;

namespace eShop.AdminApp.Service.Role
{
    public class RoleApiClient : IRoleApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoleApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<List<RoleVm>>> GetAll()
        {
            HttpClient httpClient = ServiceUtils.GetHttpClient(_configuration, _httpClientFactory, _httpContextAccessor);
            var response = await httpClient.GetAsync("roles");

            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<List<RoleVm>>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<List<RoleVm>>>(result);
        }
    }
}
