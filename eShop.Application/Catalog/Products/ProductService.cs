using eShop.Application.Common;
using eShop.Application.System.Roles;
using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.Utilities.Contants;
using eShop.ViewModels.Catalog.ProductImage;
using eShop.ViewModels.Catalog.Products.Dtos;
using eShop.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace eShop.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly IStorageService _storageService;
        private readonly EShopDbContext _dbContext;
        private readonly ILanguageService _languageService;
        public ProductService(EShopDbContext dbContext, IStorageService storageService, ILanguageService languageService)
        {
            _dbContext = dbContext;
            _storageService = storageService;
            _languageService = languageService;
        }

        public async Task<ApiResult<ProductVm>> GetById(int productId, string languageId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null) return new ApiErrorResult<ProductVm>($"Can not find product: {productId}");
            var productTranlation = await _dbContext.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId && x.LanguageId == languageId);
            var productInCategory = await _dbContext.ProductInCategories.FirstOrDefaultAsync(x => x.ProductId == productId);
            var rs = new ProductVm()
            {
                Id = product.Id,
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
                IsFeatured = product.IsFeatured,
                CategoryId = productInCategory?.CategoryId ?? 0
            };
            return new ApiSuccessResult<ProductVm>(rs);
        }

        public async Task<ApiResult<int>> Create(ProductCreateRequest request)
        {
            var productTranslation = new List<ProductTranslation>();
            var languageResult = await _languageService.GetAll();
            foreach (var lnItem in languageResult.data)
            {
                if (lnItem.Id == request.LanguageId)
                {
                    productTranslation.Add(new ProductTranslation()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoTitle = request.SeoTitle,
                        SeoAlias = request.SeoAlias,
                        LanguageId = request.LanguageId,
                    });
                }
                else
                {
                    productTranslation.Add(new ProductTranslation()
                    {
                        Name = "N/A",
                        Description = "N/A",
                        Details = "N/A",
                        SeoDescription = "N/A",
                        SeoTitle = "N/A",
                        SeoAlias = "N/A",
                        LanguageId = lnItem.Id
                    });
                }
            }
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                IsFeatured = request.IsFeatured,
                ProductTranslations = productTranslation,
                ProductInCategories = new List<ProductInCategory>()
                {
                    new ProductInCategory()
                    {
                        CategoryId = request.CategoryId
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
            return new ApiSuccessResult<int>(product.Id);
        }

        public async Task<ApiResult<int>> Update(ProductUpdateRequest request)
        {
            var product = await _dbContext.Products.FindAsync(request.Id);
            var productTranlation = await _dbContext.ProductTranslations
                .Where(x => x.ProductId == request.Id && x.LanguageId == request.LanguageId)
                .FirstOrDefaultAsync();

            if (product == null || productTranlation == null) return new ApiErrorResult<int>($"Can not find product: {request.Id}");
            else
            {
                product.IsFeatured = request.IsFeatured;
                //Save Category
                var productCategory = await _dbContext.ProductInCategories.Where(x => x.ProductId == request.Id).ToListAsync();
                if (productCategory != null)
                {
                    _dbContext.RemoveRange(productCategory);
                    _dbContext.SaveChanges();
                }
                _dbContext.ProductInCategories.Add(new ProductInCategory()
                {
                    CategoryId = request.CategoryId,
                    ProductId = request.Id
                });
                _dbContext.SaveChanges();

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

                return new ApiSuccessResult<int>(await _dbContext.SaveChangesAsync());
            }
        }

        public async Task<ApiResult<int>> Delete(int productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null) return new ApiErrorResult<int>($"Can not find product: {productId}");
            else
            {
                var images = _dbContext.ProductImages.Where(i => i.ProductId == productId);
                foreach (var image in images)
                {
                    await _storageService.DeleteFileAsync(image.ImagePath);
                }

                _dbContext.Products.Remove(product);
                return new ApiSuccessResult<int>(await _dbContext.SaveChangesAsync());
            }
        }

        public async Task<ApiResult<int>> AddViewCount(int productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null) return new ApiErrorResult<int>($"Can not find product: {productId}");
            else
            {
                product.ViewCount++;
                return new ApiSuccessResult<int>(await _dbContext.SaveChangesAsync());
            }
        }

        public async Task<ApiResult<int>> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null) return new ApiErrorResult<int>($"Can not find product: {productId}");
            else
            {
                product.Stock += addedQuantity;
                return new ApiSuccessResult<int>(await _dbContext.SaveChangesAsync());
            }
        }

        public async Task<ApiResult<int>> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null) return new ApiErrorResult<int>($"Can not find product: {productId}");
            else
            {
                product.Price = newPrice;
                return new ApiSuccessResult<int>(await _dbContext.SaveChangesAsync());
            }
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            string originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<ApiResult<int>> AddImage(int productId, ProductImageCreateRequest request)
        {
            var p = await _dbContext.Products.FindAsync(productId);
            if (p == null) return new ApiErrorResult<int>($"Can not find product: {productId}");

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

            return new ApiSuccessResult<int>(pi.Id);
        }

        public async Task<ApiResult<int>> UpdateImage(int imageId, ProductImageCreateRequest request)
        {
            var p = await _dbContext.ProductImages.FirstOrDefaultAsync(x => x.Id == imageId);
            if (p == null) return new ApiErrorResult<int>($"Can not find product image: {imageId}");

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

            return new ApiSuccessResult<int>(await _dbContext.SaveChangesAsync());
        }

        public async Task<ApiResult<int>> RemoveImage(int imageId)
        {
            var p = await _dbContext.ProductImages.FirstOrDefaultAsync(x => x.Id == imageId);
            if (p == null) return new ApiErrorResult<int>($"Can not find product image: {imageId}");

            await _storageService.DeleteFileAsync(p.ImagePath);
            _dbContext.ProductImages.Remove(p);

            return new ApiSuccessResult<int>(await _dbContext.SaveChangesAsync());
        }

        public async Task<ApiResult<List<ProductImageVm>>> GetListImages(int productId)
        {
            var p = await _dbContext.Products.FindAsync(productId);
            if (p == null) return new ApiErrorResult<List<ProductImageVm>>($"Can not find product: {productId}");

            var lst = p.ProductImages.Select(x =>
            new ProductImageVm()
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
            return new ApiSuccessResult<List<ProductImageVm>>(lst);
        }

        public async Task<ApiResult<ProductImageVm>> GetImageById(int imageId)
        {
            var x = await _dbContext.ProductImages.FirstOrDefaultAsync(x => x.Id == imageId);
            if (x == null) return new ApiErrorResult<ProductImageVm>($"Can not find product image: {imageId}");

            var viewModel = new ProductImageVm()
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
            return new ApiSuccessResult<ProductImageVm>(viewModel);
        }

        public async Task<ApiResult<PagedResult<ProductVm>>> GetAll(GetProductRequest request)
        {
            //select
            var query = from p in _dbContext.Products
                        join pic in _dbContext.ProductInCategories on p.Id equals pic.ProductId
                        join pt in _dbContext.ProductTranslations on p.Id equals pt.ProductId into ptResult
                        from pt in ptResult.DefaultIfEmpty()
                        where pt.LanguageId == request.languageId
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

            if (request.categoryId != null)
            {
                query = query.Where(x => x.pic.CategoryId == request.categoryId);
            }

            int totalCount = await query.CountAsync();

            //paging
            var data = await query.Skip((request.pageIndex - 1) * request.pageSize).Take(request.pageSize)
                .Select(x => new ProductVm()
                {
                    Id = x.p.Id,
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
                    IsFeatured = x.p.IsFeatured,
                })
                .ToListAsync();

            var result = new PagedResult<ProductVm>() { Items = data, TotalRecords = totalCount };
            return new ApiSuccessResult<PagedResult<ProductVm>>(result);
        }

        public async Task<ApiResult<List<ProductVm>>> GetListFeature(GetProductRequest request)
        {
            //select
            var query = from p in _dbContext.Products
                        join pic in _dbContext.ProductInCategories on p.Id equals pic.ProductId
                        join pt in _dbContext.ProductTranslations on p.Id equals pt.ProductId
                        where pt.LanguageId == request.languageId
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

            if (request.categoryId != null)
            {
                query = query.Where(x => x.pic.CategoryId == request.categoryId);
            }

            int totalCount = await query.CountAsync();

            //paging
            var data = await query.OrderBy(x => x.pt.Name).Skip((request.pageIndex - 1) * request.pageSize).Take(request.pageSize)
                .Select(x => new ProductVm()
                {
                    Id = x.p.Id,
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
                    IsFeatured = x.p.IsFeatured,
                })
                .ToListAsync();
            return new ApiSuccessResult<List<ProductVm>>(data);
        }

        public async Task<ApiResult<List<ProductVm>>> GetListFeature(string languageId, int take)
        {
            //select
            var query = from p in _dbContext.Products
                        join pic in _dbContext.ProductInCategories on p.Id equals pic.ProductId
                        join pt in _dbContext.ProductTranslations on p.Id equals pt.ProductId
                        join pi in _dbContext.ProductImages.Where(x => x.IsDefault) on p.Id equals pi.ProductId into piResult
                        from pi in piResult.DefaultIfEmpty()
                        where pt.LanguageId == languageId && p.IsFeatured
                        select new { p, pt, pic, pi };

            //to list
            var data = await query.OrderBy(x => x.pt.Name).Take(take)
                .Select(x => new ProductVm()
                {
                    Id = x.p.Id,
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
                    IsFeatured = x.p.IsFeatured,
                    ThumbnailImage = SystemConstants.AppSettings.ImagePath + x.pi.ImagePath
                })
                .ToListAsync();
            return new ApiSuccessResult<List<ProductVm>>(data);
        }

        public async Task<ApiResult<List<ProductVm>>> GetListLatest(string languageId, int take)
        {
            //select
            var query = from p in _dbContext.Products
                        join pic in _dbContext.ProductInCategories on p.Id equals pic.ProductId
                        join pt in _dbContext.ProductTranslations on p.Id equals pt.ProductId
                        join pi in _dbContext.ProductImages.Where(x => x.IsDefault) on p.Id equals pi.ProductId into piResult
                        from pi in piResult.DefaultIfEmpty()
                        where pt.LanguageId == languageId
                        select new { p, pt, pic, pi };

            //to list
            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(take)
                .Select(x => new ProductVm()
                {
                    Id = x.p.Id,
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
                    IsFeatured = x.p.IsFeatured,
                    ThumbnailImage = SystemConstants.AppSettings.ImagePath + x.pi.ImagePath
                })
                .ToListAsync();
            return new ApiSuccessResult<List<ProductVm>>(data);
        }
    }
}
