using eShop.Application.Common;
using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.ViewModels.Catalog.ProductImage;
using eShop.ViewModels.Catalog.Products.Dtos;
using eShop.ViewModels.Common;
using EShop.Utilities.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace eShop.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly IStorageService _storageService;
        private readonly EShopDbContext _dbContext;

        public ProductService(EShopDbContext dbContext, IStorageService storageService)
        {
            _dbContext = dbContext;
            _storageService = storageService;
        }

        public async Task<ProductViewModel> GetById(int productId, string languageId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Can not find product: {productId}");
            var productTranlation = await _dbContext.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId && x.LanguageId == languageId);
            var rs = new ProductViewModel()
            {
                DateCreated = product.DateCreated,
                Description = productTranlation?.Description,
                Details = productTranlation?.Details,
                Name = productTranlation?.Name,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                SeoAlias = productTranlation?.SeoAlias,
                SeoDescription = productTranlation?.SeoDescription,
                SeoTitle = productTranlation?.SeoTitle,
                Stock = product.Stock,
                ViewCount = product.ViewCount,
            };
            return rs;
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

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return product.Id;
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _dbContext.Products.FindAsync(request.Id);
            var productTranlation = await _dbContext.ProductTranslations
                .Where(x => x.ProductId == request.Id && x.LanguageId == request.LanguageId)
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
                    var thumbnailImage = await _dbContext.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
                    if (thumbnailImage != null)
                    {
                        thumbnailImage.FileSize = request.ThumbnailImage.Length;
                        thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                        _dbContext.ProductImages.Update(thumbnailImage);
                    }
                }

                return await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Can not find product: {productId}");
            else
            {
                var images = _dbContext.ProductImages.Where(i => i.ProductId == productId);
                foreach (var image in images)
                {
                    await _storageService.DeleteFileAsync(image.ImagePath);
                }

                _dbContext.Products.Remove(product);
                return await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<int> AddViewCount(int productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Can not find product: {productId}");
            else
            {
                product.ViewCount++;
                return await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<int> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Can not find product: {productId}");
            else
            {
                product.Stock += addedQuantity;
                return await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<int> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Can not find product: {productId}");
            else
            {
                product.Price = newPrice;
                return await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<string> SaveFile(IFormFile file)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            string originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var p = await _dbContext.Products.FindAsync(productId);
            if (p == null) throw new EShopException($"Can not find product: {productId}");

            var pi = new ProductImage()
            {
                DateCreated = DateTime.Now,
                Caption = request.Caption,
                IsDefault = false,
                SortOrder = request.SortOrder,
            };

            //Save Image
            if (request.ImageFile != null)
            {
                pi.ImagePath = await SaveFile(request.ImageFile);
                pi.FileSize = request.ImageFile.Length;
            }

            _dbContext.ProductImages.Add(pi);
            await _dbContext.SaveChangesAsync();

            return pi.Id;
        }

        public async Task<int> UpdateImage(int imageId, ProductImageCreateRequest request)
        {
            var p = await _dbContext.ProductImages.FirstOrDefaultAsync(x => x.Id == imageId);
            if (p == null) throw new EShopException($"Can not find product image: {imageId}");

            p.Caption = request.Caption;
            p.IsDefault = request.IsDefault;
            p.SortOrder = request.SortOrder;

            //Save Image
            if (request.ImageFile != null)
            {
                p.ImagePath = await SaveFile(request.ImageFile);
                p.FileSize = request.ImageFile.Length;
            }

            _dbContext.ProductImages.Update(p);

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var p = await _dbContext.ProductImages.FirstOrDefaultAsync(x => x.Id == imageId);
            if (p == null) throw new EShopException($"Can not find product image: {imageId}");

            await _storageService.DeleteFileAsync(p.ImagePath);
            _dbContext.ProductImages.Remove(p);

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ProductImageViewModel>> GetListImages(int productId)
        {
            var p = await _dbContext.Products.FindAsync(productId);
            if (p == null) throw new EShopException($"Can not find product: {productId}");

            return p.ProductImages.Select(x =>
            new ProductImageViewModel()
            {
                Id = x.Id,
                ProductId = x.ProductId,
                Caption = x.Caption,
                DateCreated = x.DateCreated,
                IsDefault = x.IsDefault,
                FileSize = x.FileSize,
                ImagePath = x.ImagePath,
                SortOrder = x.SortOrder
            }).ToList();
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var x = await _dbContext.ProductImages.FirstOrDefaultAsync(x => x.Id == imageId);
            if (x == null) throw new EShopException($"Can not find product image: {imageId}");

            var viewModel = new ProductImageViewModel()
            {
                Id = x.Id,
                ProductId = x.ProductId,
                Caption = x.Caption,
                DateCreated = x.DateCreated,
                IsDefault = x.IsDefault,
                FileSize = x.FileSize,
                ImagePath = x.ImagePath,
                SortOrder = x.SortOrder
            };
            return viewModel;
        }

        public async Task<PagedResult<ProductViewModel>> getAllPaging(string languageId, GetProductPagingRequest request)
        {
            //select
            var query = from p in _dbContext.Products
                        join pic in _dbContext.ProductInCategories on p.Id equals pic.ProductId
                        join pt in _dbContext.ProductTranslations on p.Id equals pt.ProductId
                        where pt.LanguageId == languageId
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
            return new PagedResult<ProductViewModel>() { Items = data, TotalRecord = totalCount };
        }

        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string languageId, GetProductPagingRequest request)
        {
            //select
            var query = from p in _dbContext.Products
                        join pic in _dbContext.ProductInCategories on p.Id equals pic.ProductId
                        join pt in _dbContext.ProductTranslations on p.Id equals pt.ProductId
                        where pt.LanguageId == languageId
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
            return new PagedResult<ProductViewModel>() { Items = data, TotalRecord = totalCount };
        }
    }
}
