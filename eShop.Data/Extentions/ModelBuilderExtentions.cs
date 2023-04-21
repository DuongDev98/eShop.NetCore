using eShop.Data.Entities;
using eShop.Data.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Data.Extentions
{
    public static class ModelBuilderExtentions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppConfig>().HasData(
                new AppConfig() { Key = "HomeTile", Value = "This is homepage of EShop" },
                new AppConfig() { Key = "HomeKeyword", Value = "This is keywork of EShop" },
                new AppConfig() { Key = "HomeDescription", Value = "This is description of EShop" }
                );

            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = "vi-VN", Name = "Tiếng Việt", IsDefault = true },
                new Language() { Id = "en-EN", Name = "English" }
                );

            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    IsShowOnHome = true,
                    ParentId = null,
                    SortOrder = 0,
                    Status = Status.Active
                },
                new Category()
                {
                    Id = 2,
                    IsShowOnHome = true,
                    ParentId = null,
                    SortOrder = 0,
                    Status = Status.Active
                }
                );

            modelBuilder.Entity<CategoryTranslation>().HasData(
                new CategoryTranslation()
                {
                    Id = 1,
                    CategoryId = 1,
                    Name = "Áo nam",
                    LanguageId = "vi-VN",
                    SeoAlias = "ao-nam",
                    SeoTitle = "Sản phẩm áo thời trang nam",
                    SeoDescription = "Sản phẩm áo thời trang nam"
                },
                new CategoryTranslation()
                {
                    Id = 2,
                    CategoryId = 1,
                    Name = "Men shirt",
                    LanguageId = "vi-VN",
                    SeoAlias = "men-shirt",
                    SeoTitle = "The shirt products for men",
                    SeoDescription = "The shirt products for men"
                },
                new CategoryTranslation()
                {
                    Id = 3,
                    CategoryId = 2,
                    Name = "Áo nữ",
                    LanguageId = "vi-VN",
                    SeoAlias = "ao-nu",
                    SeoTitle = "Sản phẩm áo thời trang nữ",
                    SeoDescription = "Sản phẩm áo thời trang nữ"
                },
                new CategoryTranslation()
                {
                    Id = 4,
                    CategoryId = 2,
                    Name = "Women shirt",
                    LanguageId = "vi-VN",
                    SeoAlias = "women-shirt",
                    SeoTitle = "The shirt products for women",
                    SeoDescription = "The shirt products for women",
                });

            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    Id = 1,
                    DateCreated = DateTime.Now,
                    OriginalPrice = 150000,
                    Price = 200000,
                    Stock = 0,
                    ViewCount = 0
                }
                );

            modelBuilder.Entity<ProductTranslation>().HasData(
                new ProductTranslation()
                {
                    Id = 1,
                    ProductId = 1,
                    Name = "Áo sơ mi nam trắng Việt Tiến",
                    LanguageId = "vi-VN",
                    SeoAlias = "ao-so-mi-nam-trang-viet-tien",
                    SeoTitle = "Áo sơ mi nam trắng Việt Tiến",
                    SeoDescription = "Áo sơ mi nam trắng Việt Tiến",
                    Details = "Áo sơ mi nam trắng Việt Tiến",
                    Description = ""
                },
                new ProductTranslation()
                {
                    Id = 2,
                    ProductId = 1,
                    Name = "Viet Tien Men T-Shirt",
                    LanguageId = "vi-VN",
                    SeoAlias = "viet-tien-men-t-shirt",
                    SeoTitle = "Viet Tien Men T-Shirt",
                    SeoDescription = "Viet Tien Men T-Shirt",
                    Details = "Viet Tien Men T-Shirt",
                    Description = ""
                }
                );

            modelBuilder.Entity<ProductInCategory>().HasData(
                new ProductInCategory() { CategoryId = 1, ProductId = 1 }
                );
        }
    }
}
