using eShop.Data.EF;
using eShop.ViewModels.Common;
using eShop.ViewModels.Utilities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Application.Utilities.Slides
{
    public class SlideService : ISlideService
    {
        private readonly EShopDbContext _dbContext;
        public SlideService(EShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResult<List<SlideVm>>> GetAll()
        {
            var slides = await _dbContext.Slides.Select(x =>
            new SlideVm()
            {
                Name = x.Name,
                Description = x.Description,
                Image = x.Image,
                Url = x.Url
            }).ToListAsync();

            return new ApiSuccessResult<List<SlideVm>>(slides);
        }
    }
}
