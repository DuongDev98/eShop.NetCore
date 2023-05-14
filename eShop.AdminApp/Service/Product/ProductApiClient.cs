using eShop.AdminApp.Service.Common;
using eShop.Utilities.Contants;
using eShop.ViewModels.Catalog.Products.Dtos;
using eShop.ViewModels.Common;

namespace eShop.AdminApp.Service.Product
{
    public class ProductApiClient : BaseApiClient, IProductApiClient
    {
        public ProductApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor) { }

        public async Task<ApiResult<PagedResult<ProductVm>>> GetPaging(GetProductRequest request)
        {
            return await GetAsync<PagedResult<ProductVm>>($"products?languageId={request.languageId}&pageIndex={request.pageIndex}&pageSize={request.pageSize}&keyword={request.keyword}");
        }
    }
}
