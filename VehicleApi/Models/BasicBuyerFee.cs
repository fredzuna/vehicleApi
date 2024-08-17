using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VehicleApi.Models
{
    public class BasicBuyerFee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public decimal Percentage { get; set; }

        public decimal MaximumAmount { get; set; }

        public decimal MinimunAmount { get; set; }

        // Foreign key
        public int VehicleFeeId { get; set; }

        [JsonIgnore] // Prevent circular reference
        public VehicleFee? VehicleFee { get; set; }

        // Method to calculate basic buyer price
        public decimal CalculateBasicBuyerPrice(decimal basePrice)
        {
            var basicBuyerPrice = basePrice * (Percentage / 100);
            var adjustedPrice = AdjustPriceWithinRange(basicBuyerPrice);

            return adjustedPrice;
        }

        // Method to adjust price within the defined range
        public decimal AdjustPriceWithinRange(decimal price)
        {
            if (price < MinimunAmount)
            {
                return MinimunAmount;
            }
            else if (price > MaximumAmount)
            {
                return MaximumAmount;
            }
            return price;
        }
    }
}