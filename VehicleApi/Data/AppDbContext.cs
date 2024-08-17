using Microsoft.EntityFrameworkCore;
using VehicleApi.Models;

namespace VehicleApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<VehicleFee> VehicleFees { get; set; }
        public DbSet<BasicBuyerFee> BasicBuyerFees { get; set; }
        public DbSet<AssociationFee> AssociationFees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VehicleFee>()
            .Property(v => v.Type)
            .HasConversion<string>(); // Converts the enum to string in the database

            modelBuilder.Entity<VehicleFee>()
            .HasOne(v => v.BasicBuyerFee)
            .WithOne(b => b.VehicleFee)
            .HasForeignKey<BasicBuyerFee>(v => v.VehicleFeeId);            

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var vehicleFee1 = new VehicleFee
            {
                Id = 1,
                Type = VehicleTypeEnum.Common,
                SpecialFeePercentage = 2,
                StorageFee = 100
            };

            var vehicleFee2 = new VehicleFee
            {
                Id = 2,
                Type = VehicleTypeEnum.Luxury,
                SpecialFeePercentage = 4,
                StorageFee = 100
            };

            modelBuilder.Entity<VehicleFee>().HasData(vehicleFee1, vehicleFee2);

            var basicBuyerFee1 = new BasicBuyerFee
            {
                Id = 1,
                VehicleFeeId = 1,
                Percentage = 10,
                MinimunAmount = 10,
                MaximumAmount = 50,
            };

            var basicBuyerFee2 = new BasicBuyerFee
            {
                Id = 2,
                VehicleFeeId = 2,
                Percentage = 10,
                MinimunAmount = 25,
                MaximumAmount = 200,
            };

            modelBuilder.Entity<BasicBuyerFee>().HasData(basicBuyerFee1, basicBuyerFee2);

            var associationFee1 = new AssociationFee
            {
                Id = 1,
                Amount = 5,
                StartAmount = 1,
                EndAmount = 500,
                Order = 1
            };

            var associationFee2 = new AssociationFee
            {
                Id = 2,
                Amount = 10,
                StartAmount = 500,
                EndAmount = 1000,
                Order = 2
            };

            var associationFee3 = new AssociationFee
            {
                Id = 3,
                Amount = 15,
                StartAmount = 1000,
                EndAmount = 3000,
                Order = 3
            };

            var associationFee4 = new AssociationFee
            {
                Id = 4,
                Amount = 20,
                StartAmount = 3000,
                Order = 4
            };

            modelBuilder.Entity<AssociationFee>().HasData(associationFee1, associationFee2, associationFee3, associationFee4);
        }
    }
}
