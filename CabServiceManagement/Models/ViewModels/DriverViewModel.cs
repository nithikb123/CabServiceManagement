namespace CabServiceManagement.Models.ViewModels
{
    public class DriverViewModel
    {
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
    }
}
