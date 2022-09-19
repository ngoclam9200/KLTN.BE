using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DoubleLStore.WebApp.EF
{
    public class DoubleLStoreDbContextFactory : IDesignTimeDbContextFactory<doubleLStoreDbContext>
    {
        public doubleLStoreDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("DoubelLStoreDb");
            var optionsBuilder =new DbContextOptionsBuilder<doubleLStoreDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new doubleLStoreDbContext(optionsBuilder.Options);
        }
    }
}
