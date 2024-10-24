using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Blog2024ApiApp.Data;
using Blog2024ApiApp.Enums;
using Blog2024ApiApp.Services;
using Blog2024ApiApp.Services.Interfaces;
using Blog2024Api.DTO;


namespace Blog2024ApiApp.Controllers
{
    [Authorize]

    #region PRIMARY CONSTRUCTOR
    public class UserRolesController(IRolesService rolesService,
                                 IApplicationUserService ApplicationUserService,
                                 UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly IRolesService _rolesService = rolesService;
        private readonly IApplicationUserService _applicationUserService = ApplicationUserService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        #endregion

        #region MANAGE USER ROLES
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ManageUserRoles()
        {
            //add instance of the view model
            List<ManageUserRolesDTO> model = new();

            //Get list of all users
            List<ApplicationUser> users = (List<ApplicationUser>)await _applicationUserService.GetAllUsersAsync();

            foreach (ApplicationUser user in users)
            {
                ManageUserRolesDTO viewModel = new();
                viewModel.ApplicationUser = user;
                IEnumerable<string>selected = await _rolesService.GetUserRolesAsync(user);
                viewModel.Roles = new MultiSelectList( await _rolesService.GetRolesAsync(),"Name", "Name", selected);

                model.Add(viewModel);
            }

            return View(model);
        }
        #endregion

        #region ASSIGN ROLE
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesDTO model)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(model.ApplicationUser.Id);
            if (user == null)
            {
                return NotFound();
            }

            IEnumerable<string> roles = await _rolesService.GetUserRolesAsync(user);
            string userRole = model.SelectedRoles.FirstOrDefault();

            if (!string.IsNullOrEmpty(userRole))
            {
                if (await _rolesService.RemoveUserFromRolesAsync(user, roles))
                {
                    await _rolesService.AddUserToRoleAsync(user, userRole);
                }
            }

            return RedirectToAction(nameof(ManageUserRoles));
        } 
        #endregion

    }
}
