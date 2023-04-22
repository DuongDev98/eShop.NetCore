using eShop.Application.Catalog.Products.Dtos;
using eShop.Application.Catalog.Products.Dtos.Manage;
using eShop.Application.Dtos;
using eShop.Data.EF;
using eShop.Data.Entities;
using EShop.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace eShop.Application.Catalog.Products
{
    public class MagageProductService : IManageProductService
    {
        private readonly EShopDbContext db;

        public MagageProductService(EShopDbContext db)
        {
            this.db = db;
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoTitle = request.SeoTitle,
                        SeoAlias = request.SeoAlias,
                        LanguageId = request.LanguageId,
                    }
                }
            };
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return 0;
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await db.Products.FindAsync(request.Id);
            var productTranlation = await db.ProductTranslations
                .Where(x=>x.ProductId == request.Id && x.LanguageId == request.LanguageId)
                .FirstOrDefaultAsync();
            if (product == null || productTranlation == null) throw new EShopException($"Can not find product: {request.Id}");
            else
            {
                productTranlation.Name = request.Name;
                productTranlation.SeoAlias = request.SeoAlias;
                productTranlation.Description = request.Description;
                productTranlation.Details = request.Details;
                productTranlation.SeoDescription = request.SeoDescription;
                productTranlation.SeoTitle = request.SeoTitle;
                return await db.SaveChangesAsync();
            }
        }

        public async Task<int> Delete(int productId)
        {
            var product = await db.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Can not find product: {productId}");
            else
            {
                db.Products.Remove(product);
                return await db.SaveChangesAsync();
            }
        }

        public async Task<PageResult<ProductViewModel>> getAll()
        {
            throw new NotImplementedException();
        }

        public async Task<PageResult<ProductViewModel>> getAllPaging(GetProductPagingRequest request)
        {
            //select
            var query = from p in db.Products
                        join pic in db.ProductInCategories on p.Id equals pic.ProductId
                        join pt in db.ProductTranslations on p.Id equals pt.ProductId
                        select new { p, pt, pic };
            //filter
            if (!string.IsNullOrEmpty(request.keyword))
            {
                query = query.Where(x => x.pt.Name.ToLower().Contains(request.keyword.ToLower()));
            }

            if (request.categoryIds != null && request.categoryIds.Count > 0)
            {
                query = query.Where(x => request.categoryIds.Contains(x.pic.CategoryId));
            }

            int totalCount = await query.CountAsync();

            //paging
            var data = await query.Skip((request.pageIndex - 1) * request.pageSize).Take(request.pageSize)
                .Select(x=>new ProductViewModel()
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

        public async Task AddViewCount(int productId)
        {
            var product = await db.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Can not find product: {productId}");
            else
            {
                product.ViewCount++;
                await db.SaveChangesAsync();
            }
        }

        public async Task UpdateStock(int productId, int addedQuantity)
        {
            var product = await db.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Can not find product: {productId}");
            else
            {
                product.Stock += addedQuantity;
                await db.SaveChangesAsync();
            }
        }

        public async Task UpdatePrice(int productId, decimal newPrice)
        {
            var product = await db.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Can not find product: {productId}");
            else
            {
                product.Price = newPrice;
                await db.SaveChangesAsync();
            }
        }
    }
}
