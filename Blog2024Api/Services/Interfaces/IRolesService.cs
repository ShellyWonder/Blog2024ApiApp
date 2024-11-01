using Microsoft.AspNetCore.Identity;
using Blog2024Api.Enums;
using Blog2024Api.Identity;

namespace Blog2024Api.Services.Interfaces
{
    public interface IRolesService
    {
        Task<bool> IsUserInRoleAsync(ApplicationUser user, string roleName);
        Task<List<IdentityRole>> GetRolesAsync();
        Task<bool> AddUserToRoleAsync(ApplicationUser user, string roleName);
        Task<bool> RemoveUserFromRoleAsync(ApplicationUser user, string roleName); 
        Task<bool> RemoveUserFromRolesAsync(ApplicationUser user, IEnumerable<string>roles);
        Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser user);
        Task<List<ApplicationUser>> GetUsersNotInRoleAsync(BlogRole role);
        Task<string>GetRoleNameByIdAsync(string roleId);

    }
}