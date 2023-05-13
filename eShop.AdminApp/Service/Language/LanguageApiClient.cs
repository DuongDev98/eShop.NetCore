using eShop.AdminApp.Service.Common;
using eShop.ViewModels.Common;
using eShop.ViewModels.System.Languages;

namespace eShop.AdminApp.Service.Language
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