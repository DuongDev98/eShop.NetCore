using eShop.ApiIntegration.Common;
using eShop.ViewModels.Catalog.Categories;
using eShop.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace eShop.ApiIntegration.Category
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        public CategoryApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor) { }

        public async Task<ApiResult<List<CategoryVm>>> GetAll(string languageId)
        {
            return await GetAsync<List<CategoryVm>>("categories/" + languageId.ToString());
        }
    }
}
