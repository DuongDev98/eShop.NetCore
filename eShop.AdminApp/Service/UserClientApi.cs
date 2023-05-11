using eShop.ViewModels.Common;
using eShop.ViewModels.System.Users;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace eShop.AdminApp.Service
{
    public class UserClientApi : IUserClientApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserClientApi(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            HttpClient httpClient = GetHttpClient(false);
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
            HttpClient httpClient = GetHttpClient();
            var response = await httpClient.GetAsync($"users/paging?pageIndex={request.pageIndex}&pageSize={request.pageSize}&keyword={request.keyword}");
            var body = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiResult<PagedResult<UserVm>>>(body);
        }

        public async Task<ApiResult<bool>> RegisterUser(RegisterRequest request)
        {
            HttpClient httpClient = GetHttpClient(false);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("users", content);

            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> UpdateUser(UpdateUserRequest request)
        {
            HttpClient httpClient = GetHttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync("users/" + request.Id.ToString(), content);

            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<UserVm>> GetUserById(Guid Id)
        {
            HttpClient httpClient = GetHttpClient();
            var response = await httpClient.GetAsync("users/" + Id.ToString());

            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<UserVm>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<UserVm>>(result);
        }

        private HttpClient GetHttpClient(bool useToken = true)
        {
            string baseUrl = _configuration.GetValue<string>("BaseAddress");
            if (!baseUrl.EndsWith("/")) baseUrl += "/";
            baseUrl += "api/";

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(baseUrl);

            if (useToken)
            {
                string bearerToken = _httpContextAccessor.HttpContext?.Session.GetString("Token")??"";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            }

            return httpClient;
        }
    }
}
