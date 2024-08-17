using VehicleApi.Dtos;

namespace VehicleApi.Services
{
    public interface IVehicleService
    {
        VehicleTotalPriceDto GetCalculateVehiclePrice(VehicleBasePriceDto vehiclePrice);
    }
}
