using eShop.Data.EF;
using eShop.ViewModels.Catalog.Categories;
using eShop.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace eShop.Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly EShopDbContext _dbContext;
        public CategoryService(EShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResult<List<CategoryVm>>> GetAll(string languageId)
        {
            var query = from c in _dbContext.Categories
                            join ct in _dbContext.CategoryTranslations on c.Id equals ct.CategoryId
                            where ct.LanguageId == languageId
                            select new { c, ct };
            var categories = await query.Select(x => new CategoryVm() {
                Id = x.c.Id,
                Name = x.ct.Name,
                SeoTitle = x.ct.SeoTitle,
                SeoDescription = x.ct.SeoDescription,
                SeoAlias = x.ct.SeoAlias,
                IsShowOnHome = x.c.IsShowOnHome,
                ParentId = x.c.ParentId
            }).ToListAsync();
            return new ApiSuccessResult<List<CategoryVm>>(categories);
        }

        public async Task<ApiResult<CategoryVm>> GetById(int id, string languageId)
        {
            var query = from c in _dbContext.Categories
                        join ct in _dbContext.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId == languageId && c.Id == id
                        select new { c, ct };

            var category = await query.Select(x => new CategoryVm()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                SeoTitle = x.ct.SeoTitle,
                SeoDescription = x.ct.SeoDescription,
                SeoAlias = x.ct.SeoAlias,
                IsShowOnHome = x.c.IsShowOnHome,
                ParentId = x.c.ParentId
            }).FirstOrDefaultAsync();

            return new ApiSuccessResult<CategoryVm>(category);
        }
    }
}