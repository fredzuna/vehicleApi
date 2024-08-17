using VehicleApi.Models;

namespace VehicleApi.Dtos
{
    public class VehicleBasePriceDto
    {
        public VehicleTypeEnum Type { get; set; }
        public decimal BasePrice { get; set; }        
    }
}
