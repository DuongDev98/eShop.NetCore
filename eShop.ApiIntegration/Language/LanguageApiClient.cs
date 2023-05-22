using eShop.ApiIntegration.Common;
using eShop.ViewModels.Common;
using eShop.ViewModels.System.Languages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace eShop.ApiIntegration.Language
{
    public class LanguageApiClient : BaseApiClient, ILanguageApiClient
    {
        public LanguageApiClient(IConfiguration configuration,  IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) 
            : base(configuration, httpClientFactory, httpContextAccessor) {}

        public async Task<ApiResult<List<LanguageVm>>> GetAll()
        {
            return await GetAsync<List<LanguageVm>>("languages");
        }
    }
}