using eShop.Data.Entities;
using eShop.Data.Enum;
using eShop.Utilities.Contants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
                new Language() { Id = SystemConstants.vi, Name = "Tiếng Việt", IsDefault = true },
                new Language() { Id = SystemConstants.en, Name = "English" }
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
                    LanguageId = SystemConstants.vi,
                    SeoAlias = "ao-nam",
                    SeoTitle = "Sản phẩm áo thời trang nam",
                    SeoDescription = "Sản phẩm áo thời trang nam"
                },
                new CategoryTranslation()
                {
                    Id = 2,
                    CategoryId = 1,
                    Name = "Men shirt",
                    LanguageId = SystemConstants.en,
                    SeoAlias = "men-shirt",
                    SeoTitle = "The shirt products for men",
                    SeoDescription = "The shirt products for men"
                },
                new CategoryTranslation()
                {
                    Id = 3,
                    CategoryId = 2,
                    Name = "Áo nữ",
                    LanguageId = SystemConstants.vi,
                    SeoAlias = "ao-nu",
                    SeoTitle = "Sản phẩm áo thời trang nữ",
                    SeoDescription = "Sản phẩm áo thời trang nữ"
                },
                new CategoryTranslation()
                {
                    Id = 4,
                    CategoryId = 2,
                    Name = "Women shirt",
                    LanguageId = SystemConstants.en,
                    SeoAlias = "women-shirt",
                    SeoTitle = "The shirt products for women",
                    SeoDescription = "The shirt products for women",
                });

            //modelBuilder.Entity<Product>().HasData(
            //     new Product()
            //     {
            //         Id = 1,
            //         DateCreated = DateTime.Now,
            //         OriginalPrice = 150000,
            //         Price = 200000,
            //         Stock = 0,
            //         ViewCount = 0
            //     }
            //     );

            //modelBuilder.Entity<ProductTranslation>().HasData(
            //new ProductTranslation()
            //{
            //    Id = 1,
            //    ProductId = 1,
            //    Name = "Áo sơ mi nam trắng Việt Tiến",
            //    LanguageId = SystemConstants.vi,
            //    SeoAlias = "ao-so-mi-nam-trang-viet-tien",
            //    SeoTitle = "Áo sơ mi nam trắng Việt Tiến",
            //    SeoDescription = "Áo sơ mi nam trắng Việt Tiến",
            //    Details = "Áo sơ mi nam trắng Việt Tiến",
            //    Description = ""
            //},
            //new ProductTranslation()
            //{
            //    Id = 2,
            //    ProductId = 1,
            //    Name = "Viet Tien Men T-Shirt",
            //    LanguageId = SystemConstants.en,
            //    SeoAlias = "viet-tien-men-t-shirt",
            //    SeoTitle = "Viet Tien Men T-Shirt",
            //    SeoDescription = "Viet Tien Men T-Shirt",
            //    Details = "Viet Tien Men T-Shirt",
            //    Description = ""
            //}
            //);

            //modelBuilder.Entity<ProductInCategory>().HasData(
            //    new ProductInCategory() { CategoryId = 1, ProductId = 1 }
            //    );

            //seed admin user
            var roleId = new Guid("F611BBFD-A34D-4F71-BDAF-1D373F8CB891");
            var userId = new Guid("F41F21F2-FE66-4DF9-A2BC-CFE970F45479");
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleId,
                Name = "admin",
                NormalizedName = "admin",
            });

            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = userId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "duongdev98@gmail.com",
                NormalizedEmail = "duongdev98@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "1qaz2345"),
                SecurityStamp = string.Empty,
                FirstName = "Duong",
                LastName = "Nguyen",
                Dob = new DateTime(1998, 2, 4)
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = userId
            });

            modelBuilder.Entity<Slide>().HasData(
                new Slide() { Id = 1, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", Image = "/themes/images/carousel/1.png", Url = "#" },
                new Slide() { Id = 2, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", Image = "/themes/images/carousel/2.png", Url = "#" },
                new Slide() { Id = 3, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", Image = "/themes/images/carousel/3.png", Url = "#" },
                new Slide() { Id = 4, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", Image = "/themes/images/carousel/4.png", Url = "#" },
                new Slide() { Id = 5, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", Image = "/themes/images/carousel/5.png", Url = "#" },
                new Slide() { Id = 6, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", Image = "/themes/images/carousel/6.png", Url = "#" }
            );
        }
    }
}
