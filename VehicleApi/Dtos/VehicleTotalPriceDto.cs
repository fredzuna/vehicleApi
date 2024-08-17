using VehicleApi.Models;

namespace VehicleApi.Dtos
{
    public class VehicleTotalPriceDto
    {
        public decimal TotalPrice { get; set; }
        public required VehicleChargesDto VehicleCharges { get; set; }
        
    }
}
