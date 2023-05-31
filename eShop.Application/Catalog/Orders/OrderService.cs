using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.ViewModels.Catalog.Orders;
using eShop.ViewModels.Common;
using Microsoft.Extensions.Configuration;

namespace eShop.Application.Catalog.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IConfiguration _configuration;
        private readonly EShopDbContext _dbContext;
        public OrderService(IConfiguration configuration, EShopDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<ApiResult<bool>> Create(OrderCreateRequest request)
        {
            try
            {
                Order order = new Order();
                order.OrderDate = DateTime.Now;
                order.ShipName = request.Name;
                order.ShipPhoneNumber = request.Name;
                order.ShipEmail = request.Name;
                order.ShipAddress = request.Name;
                order.UserId = request.UserId;
                order.OrderDetails = new List<OrderDetail>();

                foreach (var item in request.OrderDetais)
                {
                    OrderDetail detail = new OrderDetail();
                    detail.ProductId = item.ProductId;
                    detail.Price = item.Price;
                    detail.Quantity = item.Quantity;
                    order.OrderDetails.Add(detail);
                }

                _dbContext.Orders.Add(order);
                await _dbContext.SaveChangesAsync();
                return new ApiSuccessResult<bool>();
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>(ex.Message);
            }
        }
    }
}
