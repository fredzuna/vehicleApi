using Microsoft.EntityFrameworkCore;
using YourNamespace.Models;

namespace YourNamespace.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<VehicleFee> VehicleFees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VehicleFee>().HasData(
                new VehicleFee { Id = 1, SpecialFeePercentage = 5.0m, StorageFee = 100.0m },
                new VehicleFee { Id = 2,  SpecialFeePercentage = 7.5m, StorageFee = 150.0m }
            );
        }
    }
}
