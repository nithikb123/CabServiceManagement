using System.ComponentModel.DataAnnotations;

namespace CabServiceManagement.Models
{
    public class ApplicationUser: IdentityUser
    {
        [StringLength(20)]
        public string FirstName { get; set; }

        [StringLength(20)]
        public string LastName { get; set; }
        public IEnumerable<Booking> BookingCabs { get; set; }

    }
}
