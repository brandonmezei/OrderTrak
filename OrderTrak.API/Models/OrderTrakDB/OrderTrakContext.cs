using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class OrderTrakContext : DbContext
    {
        private readonly string _username;
        public OrderTrakContext(DbContextOptions<OrderTrakContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _username = httpContextAccessor.HttpContext?.Items["Username"]?.ToString() ?? "System";
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply global query filter to exclude deleted entities
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(CommonObject).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var filter = Expression.Lambda(
                        Expression.Equal(
                            Expression.Property(parameter, nameof(CommonObject.IsDelete)),
                            Expression.Constant(false)
                        ),
                        parameter
                    );
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
                }
            }
        }

        public override int SaveChanges()
        {
            SetDefaults();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetDefaults();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetDefaults()
        {
            var entries = ChangeTracker.Entries<CommonObject>()
                .Where(e => e.State == EntityState.Added);

            foreach (var entry in entries)
            {
                entry.Entity.CreateName = _username;
                entry.Entity.CreateDate = DateTime.UtcNow;
            }

            entries = ChangeTracker.Entries<CommonObject>()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                entry.Entity.UpdateName = _username;
                entry.Entity.UpdateDate = DateTime.UtcNow;
            }
        }

        public DbSet<SYS_User> SYS_Users { get; set; }

        public DbSet<SYS_ChangeLog> SYS_ChangeLog { get; set; }
        public DbSet<SYS_ChangeLogDetails> SYS_ChangeLogDetails { get; set; }
    }
}
