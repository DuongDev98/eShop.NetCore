using eShop.ViewModels.Dtos;
using eShop.Data.EF;
using Microsoft.EntityFrameworkCore;
using eShop.ViewModels.Catalog.Products.Dtos;
using eShop.ViewModels.Catalog.Products.Dtos.Public;

namespace eShop.Application.Catalog.Products
{
    public class PublicProductService : IPublicProductService
    {
        private readonly EShopDbContext db;

        public PublicProductService(EShopDbContext db) { this.db = db; }

        public async Task<PageResult<ProductViewModel>> GetAllByCategoryId(GetProductPagingRequest request)
        {
            //select
            var query = from p in db.Products
                        join pic in db.ProductInCategories on p.Id equals pic.ProductId
                        join pt in db.ProductTranslations on p.Id equals pt.ProductId
                        select new { p, pt, pic };
            //filter
            if (request.categoryId > 0)
            {
                query = query.Where(x => x.pic.CategoryId == request.categoryId);
            }

            int totalCount = await query.CountAsync();

            //paging
            var data = await query.Skip((request.pageIndex - 1) * request.pageSize).Take(request.pageSize)
                .Select(x => new ProductViewModel()
                {
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Description,
                    Name = x.pt.Name,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                })
                .ToListAsync();
            return new PageResult<ProductViewModel>() { Items = data, TotalRecord = totalCount };
        }
    }
}
