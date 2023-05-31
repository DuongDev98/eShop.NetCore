using eShop.ViewModels.Catalog.Orders;
using eShop.ViewModels.Common;

namespace eShop.ApiIntegration.Order
{
    public interface IOrderApiClient
    {
        public Task<ApiResult<bool>> Create(OrderCreateRequest request);
    }
}