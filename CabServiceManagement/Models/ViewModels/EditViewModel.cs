namespace CabServiceManagement.Models.ViewModels
{
    public class EditViewModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
