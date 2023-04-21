using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace eShop.Data.EF
{
    public class EShopDbContextFactory : IDesignTimeDbContextFactory<EShopDbContext>
    {
        public EShopDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot root = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            DbContextOptionsBuilder<EShopDbContext> optionsBuilder = new DbContextOptionsBuilder<EShopDbContext>();
            optionsBuilder.UseSqlServer(root.GetConnectionString("eShopDatabase"));

            return new EShopDbContext(optionsBuilder.Options);
        }
    }
}
