using eShop.ViewModels.Common;
using eShop.ViewModels.Utilities;

namespace eShop.Application.Utilities.Slides
{
    public interface ISlideService
    {
        Task<ApiResult<List<SlideVm>>> GetAll();
    }
}
