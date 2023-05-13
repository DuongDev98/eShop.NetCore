using eShop.ViewModels.Common;
using eShop.ViewModels.System.Languages;

namespace eShop.Application.System.Roles
{
    public interface ILanguageService
    {
        Task<ApiResult<List<LanguageVm>>> GetAll();
    }
}
