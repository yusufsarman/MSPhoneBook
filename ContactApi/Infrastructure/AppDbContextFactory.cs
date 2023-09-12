using Microsoft.EntityFrameworkCore;

namespace ContactApi.Infrastructure
{    
    public class AppDbContextFactory : IDbContextFactory<AppDbContext>
    {        
        public virtual AppDbContext CreateDbContext()
        {
            // You can use this method to configure and create your DbContext.
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
