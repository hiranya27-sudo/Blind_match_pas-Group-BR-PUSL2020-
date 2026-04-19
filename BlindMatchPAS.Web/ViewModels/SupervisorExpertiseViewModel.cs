using System.ComponentModel.DataAnnotations;

namespace BlindMatchPAS.Web.ViewModels
{
    public class SupervisorExpertiseViewModel
    {
        [Required]
        [Display(Name = "Research Area")]
        public int ResearchAreaId { get; set; }
    }
}