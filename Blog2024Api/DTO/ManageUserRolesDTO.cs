using Blog2024Api.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog2024Api.DTO
{
    public class ManageUserRolesDTO
    {
        public ApplicationUser? ApplicationUser { get; set; }

        public MultiSelectList? Roles { get; set; }

        public List<string>? SelectedRoles { get; set; }
    }
}
