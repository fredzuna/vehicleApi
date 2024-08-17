using VehicleApi.Models;
using System.ComponentModel.DataAnnotations;

namespace VehicleApi.Dtos
{
    public class VehicleBasePriceDto
    {
        [Required]
        public VehicleTypeEnum Type { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "The base price must be greater than 0.")]
        public decimal BasePrice { get; set; }
    }
}
