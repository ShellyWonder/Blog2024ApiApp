using Blog2024Api.Data;
using Microsoft.AspNetCore.Identity;

namespace Blog2024Api.Models
{
    public class UserRole : IdentityUserRole<int>
    {

        public required ApplicationUser User { get; set; } = null!;

        public required Role Role { get; set; } = null!;
    }
}
