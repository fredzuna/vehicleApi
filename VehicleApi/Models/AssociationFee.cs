using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleApi.Models
{
    public class AssociationFee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal StartAmount { get; set; }
        public decimal? EndAmount { get; set; }
        public int Order { get; set; }
    }
}