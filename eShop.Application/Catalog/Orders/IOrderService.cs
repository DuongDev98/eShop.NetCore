using eShop.ViewModels.Catalog.Orders;
using eShop.ViewModels.Common;

namespace eShop.Application.Catalog.Orders
{
    public interface IOrderService
    {
        public Task<ApiResult<bool>> Create(OrderCreateRequest request);
    }
}
