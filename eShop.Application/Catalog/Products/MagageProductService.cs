using eShop.ViewModels.Catalog.Products.Dtos;
using eShop.ViewModels.Catalog.Products.Dtos.Manage;
using eShop.ViewModels.Dtos;
using eShop.Data.EF;
using eShop.Data.Entities;
using EShop.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using Azure.Core;
using eShop.Application.Common;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace eShop.Application.Catalog.Products
{
    public class MagageProductService : IManageProductService
    {
        private readonly IStorageService storageService;
        private readonly EShopDbContext db;

        public MagageProductService(EShopDbContext db, IStorageService storageService)
        {
            this.db = db;
            this.storageService = storageService;
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

            //Save Image
            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        DateCreated = DateTime.Now,
                        Caption = request.Name,
                        ImagePath = await SaveFile(request.ThumbnailImage),
                        FileSize = request.ThumbnailImage.Length,
                        IsDefault = true,
                        SortOrder = 1,
                    }
                };
            }

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

                //Save image
                if (request.ThumbnailImage != null)
                {
                    var thumbnailImage = await db.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
                    if (thumbnailImage != null)
                    {
                        thumbnailImage.FileSize = request.ThumbnailImage.Length;
                        thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                        db.ProductImages.Update(thumbnailImage);
                    }
                }

                return await db.SaveChangesAsync();
            }
        }

        public async Task<int> Delete(int productId)
        {
            var product = await db.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Can not find product: {productId}");
            else
            {
                var images = db.ProductImages.Where(i => i.ProductId == productId);
                foreach (var image in images)
                {
                    await storageService.DeleteFileAsync(image.ImagePath);
                }

                db.Products.Remove(product);
                return await db.SaveChangesAsync();
            }
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

        public async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<int> AddImages(int productId, List<IFormFile> files)
        {
            var p = await db.Products.FindAsync(productId);
            if (p == null) throw new EShopException($"Can not find product: {productId}");

            int count = p.ProductImages.Count;
            foreach (IFormFile f in files)
            {
                if (f != null && f.Length > 0)
                {
                    count++;
                    var pi = new ProductImage()
                    {
                        DateCreated = DateTime.Now,
                        Caption = "",
                        ImagePath = await SaveFile(f),
                        FileSize = f.Length,
                        IsDefault = false,
                        SortOrder = count,
                    };
                }
            }
            return count - p.ProductImages.Count;
        }

        public async Task<int> RemoveImages(int imageId)
        {
            var p = await db.ProductImages.FindAsync(imageId);
            if (p == null) throw new EShopException($"Can not find product image: {imageId}");

            await storageService.DeleteFileAsync(p.ImagePath);
            db.ProductImages.Remove(p);

            return await db.SaveChangesAsync();
        }

        public async Task<int> UpdateImage(int imageId, string caption, bool isDefault)
        {
            var p = await db.ProductImages.FindAsync(imageId);
            if (p == null) throw new EShopException($"Can not find product image: {imageId}");

            p.Caption = caption;
            p.IsDefault = isDefault;
            db.ProductImages.Update(p);

            return await db.SaveChangesAsync();
        }

        public async Task<List<ProductImageViewModel>> GetListImage(int productId)
        {
            var p = await db.Products.FindAsync(productId);
            if (p == null) throw new EShopException($"Can not find product: {productId}");

            return p.ProductImages.Select(x => 
            new ProductImageViewModel()
            {
                Caption = x.Caption,
                DateCreated = x.DateCreated,
                IsDefault = x.IsDefault,
                FileSize = x.FileSize,
                ImagePath = x.ImagePath,
                SortOrder = x.SortOrder
            }).ToList();
        }
    }
}
