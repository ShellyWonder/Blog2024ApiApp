using Microsoft.AspNetCore.Identity;

namespace Blog2024Api.Identity
{
    public class Role : IdentityRole<Guid>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
