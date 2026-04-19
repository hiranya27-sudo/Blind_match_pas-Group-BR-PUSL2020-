using BlindMatchPAS.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlindMatchPAS.Core.Entities
{
    public class Match
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project? Project { get; set; }

        public string SupervisorId { get; set; } = string.Empty;

        [ForeignKey(nameof(SupervisorId))]
        public ApplicationUser? Supervisor { get; set; }

        public MatchStatus Status { get; set; } = MatchStatus.Interested;

        public bool IdentityRevealed { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}