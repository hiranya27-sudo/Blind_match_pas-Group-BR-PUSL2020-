using System.ComponentModel.DataAnnotations;

namespace BlindMatchPAS.Web.ViewModels
{
    public class ManageProfileViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
    }
}