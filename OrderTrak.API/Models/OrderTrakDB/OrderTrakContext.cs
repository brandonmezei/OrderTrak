using Microsoft.EntityFrameworkCore;

namespace YourNamespace
{
    public class OrderTrakContext : DbContext
    {
        public DbSet<SYS_User> SYS_Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("OrderTrakDatabase"));
        }
    }
}
