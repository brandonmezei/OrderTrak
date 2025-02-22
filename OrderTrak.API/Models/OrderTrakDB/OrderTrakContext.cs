using Microsoft.EntityFrameworkCore;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class OrderTrakContext : DbContext
    {
        public OrderTrakContext(DbContextOptions<OrderTrakContext> options)
            : base(options)
        {
        }

        public DbSet<SYS_User> SYS_Users { get; set; }
    }
}
