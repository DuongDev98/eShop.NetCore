using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.ViewModels.Common;
using eShop.ViewModels.System.Languages;
using eShop.ViewModels.System.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eShop.Application.System.Roles
{
    public class LanguageService : ILanguageService
    {
        private readonly EShopDbContext _dbContext;
        public LanguageService(EShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResult<List<LanguageVm>>> GetAll()
        {
            var roles = await _dbContext.Languages.Select(x=>
            new LanguageVm() {
                Id = x.Id.ToString(),
                Name = x.Name,
                IsDefault = x.IsDefault
            }).ToListAsync();

            return new ApiSuccessResult<List<LanguageVm>>(roles);
        }
    }
}
