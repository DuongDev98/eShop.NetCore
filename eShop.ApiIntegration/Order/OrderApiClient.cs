using eShop.ApiIntegration.Common;
using eShop.ViewModels.Catalog.Orders;
using eShop.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace eShop.ApiIntegration.Order
{
    public class OrderApiClient : BaseApiClient, IOrderApiClient
    {
        public OrderApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
          : base(configuration, httpClientFactory, httpContextAccessor) { }

        public async Task<ApiResult<bool>> Create(OrderCreateRequest request)
        {
            return await PostAsync<bool>("orders", request);
        }
    }
}