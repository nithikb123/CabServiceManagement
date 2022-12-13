using System.ComponentModel.DataAnnotations.Schema;

namespace CabServiceManagement.Models
{
    [Index("LicenceNumber", IsUnique = true)]
    [Index("CabNumber", IsUnique = true)]
    public class DriverDetails
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string LicenceNumber { get; set; }
        [Required]
        [StringLength(10)]
        public string CabNumber { get; set; }
        [Required]
        [StringLength(10)]
        public string CabName { get; set; }
        public CarModel Model { get; set; }
        public ApplicationUser Driver { get; set; }
        [ForeignKey(nameof(Driver))]
        public string DriverId { get; set; }
    }
}
