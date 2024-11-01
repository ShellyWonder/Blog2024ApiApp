using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Blog2024Api.Services.Interfaces;
using Blog2024Api.DTO;
using Blog2024Api.Identity;


namespace Blog2024Api.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly UserManager<ApplicationUser> _userManager;

        #region CONSTRUCTOR
        public UserRolesController(IRolesService rolesService,
                                     IApplicationUserService ApplicationUserService,
                                     UserManager<ApplicationUser> userManager)
        {
            _rolesService = rolesService;
            _applicationUserService = ApplicationUserService;
            _userManager = userManager;
        }
        #endregion

        #region MANAGE USER ROLES
        // GET: api/UserRoles/ManageUserRoles
        [HttpGet("ManageUserRoles")]
        public async Task<ActionResult<IEnumerable<ManageUserRolesDTO>>> ManageUserRoles()
        {
            //add instance of the view model
            List<ManageUserRolesDTO> model = new();

            //Get list of all users
            List<ApplicationUser> users = (List<ApplicationUser>)await _applicationUserService.GetAllUsersAsync();

            foreach (ApplicationUser user in users)
            {
                ManageUserRolesDTO viewModel = new();
                viewModel.ApplicationUser = user;
                IEnumerable<string> selected = await _rolesService.GetUserRolesAsync(user);
                viewModel.Roles = new MultiSelectList(await _rolesService.GetRolesAsync(), "Name", "Name", selected);

                model.Add(viewModel);
            }

            return Ok(model);
        }
        #endregion

        #region ASSIGN ROLE
        // POST: api/UserRoles/ManageUserRoles
        [HttpPost("ManageUserRoles")]
        public async Task<IActionResult> ManageUserRoles([FromBody] ManageUserRolesDTO model)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(model.ApplicationUser?.Id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            IEnumerable<string> roles = await _rolesService.GetUserRolesAsync(user);
            string userRole = model.SelectedRoles?.FirstOrDefault() ?? "Commentator";

            if (!string.IsNullOrEmpty(userRole))
            {
                if (await _rolesService.RemoveUserFromRolesAsync(user, roles))
                {
                    await _rolesService.AddUserToRoleAsync(user, userRole);
                }
            }

            return NoContent();
        }
        #endregion
    }
}
