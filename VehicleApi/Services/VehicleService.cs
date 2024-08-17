using VehicleApi.Data;
using VehicleApi.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VehicleApi.Dtos;

namespace VehicleApi.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<VehicleService> _logger;

        public VehicleService(AppDbContext context, ILogger<VehicleService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public VehicleTotalPriceDto GetCalculateVehiclePrice(VehicleBasePriceDto vehicleFee)
        {
            try
            {
                var fee = _context.VehicleFees
                    .Include(fee => fee.BasicBuyerFee)
                    .FirstOrDefault(fee => fee.Type == vehicleFee.Type);

                if (fee == null)
                {
                    _logger.LogWarning("No fee found for vehicle type: {VehicleType}", vehicleFee.Type);
                    throw new Exception($"No fee found for vehicle type: {vehicleFee.Type}");
                }

                if (fee.BasicBuyerFee == null)
                {
                    _logger.LogWarning("BasicBuyerFee is null for vehicle type: {VehicleType}", vehicleFee.Type);
                    throw new Exception("BasicBuyerFee is not configured.");
                }

                var associationFees = _context.AssociationFees.OrderBy(a => a.Order).ToList();
                var calculator = new VehicleFeeCalculator(fee, fee.BasicBuyerFee, associationFees);

                var totalPrice = calculator.CalculateTotalPrice(vehicleFee.BasePrice);
                var vehicleCharges = calculator.GetVehicleCharges(vehicleFee.BasePrice);

                return new VehicleTotalPriceDto
                {
                    TotalPrice = totalPrice,
                    VehicleCharges = vehicleCharges
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating the vehicle price for vehicle type: {VehicleType}", vehicleFee.Type);
                throw new Exception("An error occurred while processing your request.");
            }
        }
    }
}
