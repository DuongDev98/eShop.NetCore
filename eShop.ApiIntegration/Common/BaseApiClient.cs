using eShop.Utilities.Contants;
using eShop.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace eShop.ApiIntegration.Common
{
    public class BaseApiClient
    {
        protected readonly IHttpClientFactory _httpClientFactory;
        protected readonly IConfiguration _configuration;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        protected BaseApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        protected async Task<ApiResult<T>> GetAsync<T>(string url)
        {
            return await RequestDataAsync<T>(url, null, HttpMethods.Get);
        }

        protected async Task<ApiResult<T>> PostAsync<T>(string url, object data)
        {
            return await RequestDataAsync<T>(url, data, HttpMethods.Post);
        }

        protected async Task<ApiResult<T>> PutAsync<T>(string url, object data)
        {
            return await RequestDataAsync<T>(url, data, HttpMethods.Put);
        }

        protected async Task<ApiResult<T>> DeleteAsync<T>(string url)
        {
            return await RequestDataAsync<T>(url, null, HttpMethods.Delete);
        }

        protected async Task<ApiResult<TResponse>> RequestDataAsync<TResponse>(string url, object data, string method)
        {
            HttpClient httpClient = GetHttpClient(_configuration, _httpClientFactory, _httpContextAccessor);

            StringContent content = null;
            if (data != null)
            {
                content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            }

            HttpResponseMessage response = null;
            if (method == HttpMethods.Get) response = await httpClient.GetAsync(url);
            else if (method == HttpMethods.Post) response = await httpClient.PostAsync(url, content);
            else if (method == HttpMethods.Put) response = await httpClient.PutAsync(url, content);
            if (method == HttpMethods.Delete) response = await httpClient.DeleteAsync(url);

            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<TResponse>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<TResponse>>(result);
        }

        protected HttpClient GetHttpClient(IConfiguration _configuration, IHttpClientFactory _httpClientFactory,
            IHttpContextAccessor _httpContextAccessor)
        {
            string baseUrl = _configuration.GetValue<string>(SystemConstants.AppSettings.BaseAddress);
            if (!baseUrl.EndsWith("/")) baseUrl += "/";
            baseUrl += "api/";

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(baseUrl);

            //add token
            string bearerToken = _httpContextAccessor.HttpContext?.Session.GetString(SystemConstants.AppSettings.Token) ?? "";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            return httpClient;
        }
    }
}
