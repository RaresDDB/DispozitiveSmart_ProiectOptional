using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineEnergyUtilityPlatform.Models
{
    public class Measurement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public double Consumption { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; }
        [Required]
        public int DeviceId { get; set; }
        [ForeignKey("DeviceId")]
        public virtual Device? Device { get; set; }
    }
}
