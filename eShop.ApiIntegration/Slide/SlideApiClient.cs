using eShop.ApiIntegration.Common;
using eShop.ViewModels.Common;
using eShop.ViewModels.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace eShop.ApiIntegration.Slide
{
    public class SlideApiClient : BaseApiClient, ISlideApiClient
    {
        public SlideApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor) { }

        public async Task<ApiResult<List<SlideVm>>> GetAll()
        {
            return await GetAsync<List<SlideVm>>("slides");
        }
    }
}
