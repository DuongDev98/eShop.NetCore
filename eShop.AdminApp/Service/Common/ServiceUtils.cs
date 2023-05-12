using System.Net.Http.Headers;

namespace eShop.AdminApp.Service.Common
{
    public class ServiceUtils
    {
        public static HttpClient GetHttpClient(IConfiguration _configuration, IHttpClientFactory _httpClientFactory,
            IHttpContextAccessor _httpContextAccessor, bool useToken = true)
        {
            string baseUrl = _configuration.GetValue<string>("BaseAddress");
            if (!baseUrl.EndsWith("/")) baseUrl += "/";
            baseUrl += "api/";

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(baseUrl);

            if (useToken)
            {
                string bearerToken = _httpContextAccessor.HttpContext?.Session.GetString("Token") ?? "";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            }

            return httpClient;
        }
    }
}
