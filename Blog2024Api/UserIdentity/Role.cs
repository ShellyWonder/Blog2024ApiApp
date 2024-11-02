using Microsoft.AspNetCore.Identity;

namespace Blog2024Api.UserIdentity
{
    public class Role : IdentityRole<Guid>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
