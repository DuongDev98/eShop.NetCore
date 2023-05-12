using eShop.AdminApp.Service.Common;
using eShop.ViewModels.Common;
using eShop.ViewModels.System.Roles;
using eShop.ViewModels.System.Users;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace eShop.AdminApp.Service.User
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            HttpClient httpClient = ServiceUtils.GetHttpClient(_configuration, _httpClientFactory, _httpContextAccessor, false);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("users/authenticate", content);

            string data = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(data);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(data);
        }

        public async Task<ApiResult<PagedResult<UserVm>>> GetUsersPaging(GetUsersPagingRequest request)
        {
            HttpClient httpClient = ServiceUtils.GetHttpClient(_configuration, _httpClientFactory, _httpContextAccessor);
            var response = await httpClient.GetAsync($"users/paging?pageIndex={request.pageIndex}&pageSize={request.pageSize}&keyword={request.keyword}");
            var body = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiResult<PagedResult<UserVm>>>(body);
        }

        public async Task<ApiResult<bool>> RegisterUser(RegisterRequest request)
        {
            HttpClient httpClient = ServiceUtils.GetHttpClient(_configuration, _httpClientFactory, _httpContextAccessor, false);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("users", content);

            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> UpdateUser(UserUpdateRequest request)
        {
            HttpClient httpClient = ServiceUtils.GetHttpClient(_configuration, _httpClientFactory, _httpContextAccessor);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync("users/" + request.Id.ToString(), content);

            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> DeleteUser(Guid Id)
        {
            HttpClient httpClient = ServiceUtils.GetHttpClient(_configuration, _httpClientFactory, _httpContextAccessor);
            var response = await httpClient.DeleteAsync("users/" + Id.ToString());

            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<UserVm>> GetUserById(Guid Id)
        {
            HttpClient httpClient = ServiceUtils.GetHttpClient(_configuration, _httpClientFactory, _httpContextAccessor);
            var response = await httpClient.GetAsync("users/" + Id.ToString());

            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<UserVm>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<UserVm>>(result);
        }

        public async Task<ApiResult<bool>> RoleAssign(Guid Id, RoleAssignRequest request)
        {
            HttpClient httpClient = ServiceUtils.GetHttpClient(_configuration, _httpClientFactory, _httpContextAccessor);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"users/{Id}/roles", content);

            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
    }
}
