using BlindMatchPAS.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlindMatchPAS.Core.Entities
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Abstract { get; set; } = string.Empty;

        [Required]
        public int ResearchAreaId { get; set; }

        [ForeignKey(nameof(ResearchAreaId))]
        public ResearchArea? ResearchArea { get; set; }

        [Required]
        public string StudentId { get; set; } = string.Empty;

        [ForeignKey(nameof(StudentId))]
        public ApplicationUser? Student { get; set; }

        public ProjectStatus Status { get; set; } = ProjectStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}