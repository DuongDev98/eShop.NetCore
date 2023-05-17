using eShop.ViewModels.Common;
using eShop.ViewModels.Utilities;

namespace eShop.ApiIntegration.Slide
{
    public interface ISlideApiClient
    {
        Task<ApiResult<List<SlideVm>>> GetAll();
    }
}
