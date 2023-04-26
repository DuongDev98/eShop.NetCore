using eShop.ViewModels.System.Users;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace eShop.AdminApp.Service
{
    public class UserClientApi : IUserClientApi
    {
        IHttpClientFactory _httpClientFactory;
        public UserClientApi(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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

        private HttpClient GetHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("http://localhost:5138/api/");
            return httpClient;
        }
    }
}
