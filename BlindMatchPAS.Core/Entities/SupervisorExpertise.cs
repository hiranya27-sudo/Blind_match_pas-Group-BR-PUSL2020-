using System.ComponentModel.DataAnnotations.Schema;

namespace BlindMatchPAS.Core.Entities
{
    public class SupervisorExpertise
    {
        public int Id { get; set; }

        public string SupervisorId { get; set; } = string.Empty;

        [ForeignKey(nameof(SupervisorId))]
        public ApplicationUser? Supervisor { get; set; }

        public int ResearchAreaId { get; set; }

        [ForeignKey(nameof(ResearchAreaId))]
        public ResearchArea? ResearchArea { get; set; }
    }
}