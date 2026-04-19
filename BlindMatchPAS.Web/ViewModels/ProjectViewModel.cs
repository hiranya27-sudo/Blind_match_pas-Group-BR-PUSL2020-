using System.ComponentModel.DataAnnotations;

namespace BlindMatchPAS.Web.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Abstract { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Research Area")]
        public int ResearchAreaId { get; set; }
    }
}