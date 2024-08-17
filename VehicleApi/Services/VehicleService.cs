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

                var associationFees = _context.AssociationFees.OrderBy(association => association.Order).ToList();

                if (fee == null || fee.BasicBuyerFee == null)
                {
                    _logger.LogWarning("No fee found for vehicle type: {VehicleType}", vehicleFee.Type);
                    throw new Exception($"No fee found for vehicle type: {vehicleFee.Type}");
                }

                var basePrice = vehicleFee.BasePrice;
                var basicBuyerPrice = fee.BasicBuyerFee.CalculateBasicBuyerPrice(basePrice);
                var specialPrice = basePrice * (fee.SpecialFeePercentage / 100);
                var associationPrice = GetAssociationPrice(basePrice, associationFees);
                var storagePrice = fee.StorageFee;

                var totalPrice = basePrice + basicBuyerPrice + specialPrice + associationPrice + storagePrice;

                var vehicleTotalPrice = new VehicleTotalPriceDto
                {
                    TotalPrice = totalPrice,
                    VehicleCharges = new VehicleChargesDto
                    {
                        Basic = basicBuyerPrice,
                        Special = specialPrice,
                        Association = associationPrice,
                        Storage = storagePrice
                    }
                };

                return vehicleTotalPrice;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating the vehicle price for vehicle type: {VehicleType}", vehicleFee.Type);
                throw new Exception("An error occurred while processing your request.");
            }
        }

        public decimal GetAssociationPrice(decimal basePrice, List<AssociationFee> associationFees)
        {
            foreach (var fee in associationFees)
            {
                if (basePrice >= fee.StartAmount &&
                    (fee.EndAmount == null || basePrice <= fee.EndAmount.Value))
                {
                    return fee.Amount;
                }
            }

            return 0;
        }

    }
}
