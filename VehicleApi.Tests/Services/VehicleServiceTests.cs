using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VehicleApi.Data;
using VehicleApi.Dtos;
using VehicleApi.Models;
using VehicleApi.Services;

public class VehicleServiceTests
{
    private readonly VehicleService _vehicleService;
    private readonly Mock<ILogger<VehicleService>> _mockLogger;
    private readonly AppDbContext _context;

    public VehicleServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "VehicleDb")            
            .Options;

        _context = new AppDbContext(options);
        _mockLogger = new Mock<ILogger<VehicleService>>();
        _vehicleService = new VehicleService(_context, _mockLogger.Object);

        SeedDatabase();
    }

    private void SeedDatabase()
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

        _context.VehicleFees.AddRange(vehicleFee1, vehicleFee2);
        _context.BasicBuyerFees.AddRange(basicBuyerFee1, basicBuyerFee2);
        _context.AssociationFees.AddRange(associationFee1, associationFee2, associationFee3, associationFee4);
        _context.SaveChanges();
    }

    [Fact]
    public void GetCalculateVehiclePrice_WithBasePrice()
    {
        // BasePrice = 398, Type = Common
        var vehicleBasePrice1 = new VehicleBasePriceDto
        {
            Type = VehicleTypeEnum.Common,
            BasePrice = 398
        };

        var result1 = _vehicleService.GetCalculateVehiclePrice(vehicleBasePrice1);

        Assert.NotNull(result1);
        Assert.Equal(550.76m, result1.TotalPrice);
        Assert.Equal(39.80m, result1.VehicleCharges.Basic);
        Assert.Equal(7.96m, result1.VehicleCharges.Special);
        Assert.Equal(5, result1.VehicleCharges.Association);
        Assert.Equal(100, result1.VehicleCharges.Storage);

        // BasePrice = 501, Type = Common
        var vehicleBasePrice2 = new VehicleBasePriceDto
        {
            Type = VehicleTypeEnum.Common,
            BasePrice = 501
        };

        var result2 = _vehicleService.GetCalculateVehiclePrice(vehicleBasePrice2);

        Assert.NotNull(result2);
        Assert.Equal(671.02m, result2.TotalPrice);
        Assert.Equal(50, result2.VehicleCharges.Basic);
        Assert.Equal(10.02m, result2.VehicleCharges.Special);
        Assert.Equal(10, result2.VehicleCharges.Association);
        Assert.Equal(100, result2.VehicleCharges.Storage);    

        // BasePrice = 57, Type = Common
        var vehicleBasePrice3 = new VehicleBasePriceDto
        {
            Type = VehicleTypeEnum.Common,
            BasePrice = 57
        };

        var result3 = _vehicleService.GetCalculateVehiclePrice(vehicleBasePrice3);

        Assert.NotNull(result3);
        Assert.Equal(173.14m, result3.TotalPrice);
        Assert.Equal(10, result3.VehicleCharges.Basic);
        Assert.Equal(1.14m, result3.VehicleCharges.Special);
        Assert.Equal(5, result3.VehicleCharges.Association);
        Assert.Equal(100, result3.VehicleCharges.Storage);

        // BasePrice = 1800, Type = Luxury
        var vehicleBasePrice4 = new VehicleBasePriceDto
        {
            Type = VehicleTypeEnum.Luxury,
            BasePrice = 1800
        };

        var result4 = _vehicleService.GetCalculateVehiclePrice(vehicleBasePrice4);

        Assert.NotNull(result4);
        Assert.Equal(2167, result4.TotalPrice);
        Assert.Equal(180, result4.VehicleCharges.Basic);
        Assert.Equal(72, result4.VehicleCharges.Special);
        Assert.Equal(15, result4.VehicleCharges.Association);
        Assert.Equal(100, result4.VehicleCharges.Storage);     

        // BasePrice = 1100, Type = Common
        var vehicleBasePrice5 = new VehicleBasePriceDto
        {
            Type = VehicleTypeEnum.Common,
            BasePrice = 1100
        };

        var result5 = _vehicleService.GetCalculateVehiclePrice(vehicleBasePrice5);

        Assert.NotNull(result5);
        Assert.Equal(1287, result5.TotalPrice);
        Assert.Equal(50, result5.VehicleCharges.Basic);
        Assert.Equal(22, result5.VehicleCharges.Special);
        Assert.Equal(15, result5.VehicleCharges.Association);
        Assert.Equal(100, result5.VehicleCharges.Storage);

        // BasePrice = 1100, Type = Common
        var vehicleBasePrice6 = new VehicleBasePriceDto
        {
            Type = VehicleTypeEnum.Luxury,
            BasePrice = 1000000
        };

        var result6 = _vehicleService.GetCalculateVehiclePrice(vehicleBasePrice6);

        Assert.NotNull(result6);
        Assert.Equal(1040320, result6.TotalPrice);
        Assert.Equal(200, result6.VehicleCharges.Basic);
        Assert.Equal(40000, result6.VehicleCharges.Special);
        Assert.Equal(20, result6.VehicleCharges.Association);
        Assert.Equal(100, result6.VehicleCharges.Storage);
    }    
}
