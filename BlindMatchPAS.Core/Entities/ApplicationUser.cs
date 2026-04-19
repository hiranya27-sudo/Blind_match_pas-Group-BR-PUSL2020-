using Microsoft.AspNetCore.Identity;

namespace BlindMatchPAS.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}