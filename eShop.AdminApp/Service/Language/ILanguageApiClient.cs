using eShop.ViewModels.Common;
using eShop.ViewModels.System.Languages;

namespace eShop.AdminApp.Service.Language
{
    public interface ILanguageApiClient
    {
        Task<ApiResult<List<LanguageVm>>> GetAll();
    }
}