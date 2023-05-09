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
        public UserClientApi(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<string> Authenticate(LoginRequest request)
        {
            HttpClient httpClient = GetHttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("users/authenticate", content);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return "";
        }

        public async Task<PagedResult<UserVm>> GetUsersPaging(GetUsersPagingRequest request)
        {
            HttpClient httpClient = GetHttpClient(request.BearerToken??"");
            var response = await httpClient.GetAsync($"users/paging?pageIndex={request.pageIndex}&pageSize={request.pageSize}&keyword={request.keyword}");
            var body = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<PagedResult<UserVm>>(body);
            return users;
        }

        public async Task<bool> RegisterUser(RegisterRequest request)
        {
            HttpClient httpClient = GetHttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("users", content);
            return response.StatusCode == HttpStatusCode.OK;
        }

        private HttpClient GetHttpClient(string bearerToken = "")
        {
            string baseUrl = _configuration.GetValue<string>("BaseAddress");
            if (!baseUrl.EndsWith("/")) baseUrl += "/";
            baseUrl += "api/";

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(baseUrl);

            if (bearerToken != null && bearerToken.Length > 0)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            }

            return httpClient;
        }
    }
}
