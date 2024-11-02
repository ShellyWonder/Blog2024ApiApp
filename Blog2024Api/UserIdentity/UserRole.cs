using Microsoft.AspNetCore.Identity;

namespace Blog2024Api.UserIdentity
{
    public class UserRole : IdentityUserRole<Guid>
    {

        public required ApplicationUser User { get; set; } = null!;

        public required Role Role { get; set; } = null!;
    }
}
