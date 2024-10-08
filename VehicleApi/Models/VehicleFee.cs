using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleApi.Models
{
    public class VehicleFee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required VehicleTypeEnum Type { get; set; }
        public decimal SpecialFeePercentage { get; set; }
        public decimal StorageFee { get; set; }
        public BasicBuyerFee? BasicBuyerFee { get; set; }
    }
}