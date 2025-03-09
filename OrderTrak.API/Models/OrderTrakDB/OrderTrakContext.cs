using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class OrderTrakContext : DbContext
    {
        private readonly IHttpContextAccessor HttpContextAccessor;

        public OrderTrakContext(DbContextOptions<OrderTrakContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            HttpContextAccessor = httpContextAccessor;
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

            // Configure foreign key constraint for SYS_RolesToFunction
            modelBuilder.Entity<SYS_RolesToFunction>()
                .HasOne(pl => pl.SYS_Function)
                .WithMany(pp => pp.SYS_RolesToFunction)
                .HasForeignKey(pl => pl.FunctionID)
                .OnDelete(DeleteBehavior.Restrict);
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
            var username = HttpContextAccessor.HttpContext?.Items["Username"]?.ToString() ?? "System";

            var entries = ChangeTracker.Entries<CommonObject>()
                .Where(e => e.State == EntityState.Added);

            foreach (var entry in entries)
            {
                entry.Entity.CreateName = username;
                entry.Entity.CreateDate = DateTime.UtcNow;
            }

            entries = ChangeTracker.Entries<CommonObject>()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                entry.Entity.UpdateName = username;
                entry.Entity.UpdateDate = DateTime.UtcNow;
            }
        }

        public string GetLoggedInUsername()
        {
            return HttpContextAccessor.HttpContext?.Items["Username"]?.ToString() ?? "System";
        }

        public DbSet<SYS_User> SYS_Users { get; set; }

        public DbSet<SYS_ChangeLog> SYS_ChangeLog { get; set; }
        public DbSet<SYS_ChangeLogDetails> SYS_ChangeLogDetails { get; set; }
        public DbSet<PO_Header> PO_Header { get; set; }
        public DbSet<PO_Line> PO_Line { get; set; }
        public DbSet<UPL_Customer> UPL_Customer { get; set; }
        public DbSet<UPL_PartInfo> UPL_PartInfo { get; set; }
        public DbSet<UPL_Project> UPL_Project { get; set; }
        public DbSet<SYS_Roles> SYS_Roles { get; set; }
        public DbSet<SYS_RolesToFunction> SYS_RolesToFunction { get; set; }
        public DbSet<SYS_Function> SYS_Function { get; set; }
        public DbSet<UPL_Location> UPL_Location { get; set; }
    }
}
