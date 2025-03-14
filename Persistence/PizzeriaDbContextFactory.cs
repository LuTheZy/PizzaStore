using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PizzaStore.Persistence
{
    public class PizzeriaDbContextFactory : IDesignTimeDbContextFactory<PizzeriaDbContext>
    {
        public PizzeriaDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<PizzeriaDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("PizzeriaDbConnection"));
            var dbContext = new PizzeriaDbContext(optionsBuilder.Options);

            return dbContext;
        }
    }
}
