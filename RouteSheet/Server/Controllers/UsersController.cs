using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RouteSheet.Data.Exceptions;
using RouteSheet.Shared;
using RouteSheet.Shared.Models;
using RouteSheet.Shared.ViewModels;

namespace RouteSheet.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("all")]
        public ActionResult<IList<UserViewModel>> GetUsers()
        {
            var users = _userManager.Users.AsQueryable();
            var usersViewModels = users.Select(x => x.ToUserViewModel());

            return Ok(usersViewModels.ToList());
        }

        [HttpPost("add")]
        public async Task<ActionResult<UserViewModel>> Add(UserAddViewModel userViewModel)
        {
            try
            {
                var appUser = userViewModel.ToAppUser();
                await _userManager.CreateAsync(appUser, userViewModel.Password);
                await _userManager.AddToRoleAsync(appUser, userViewModel.Role);

                return Ok(appUser.ToUserViewModel());
            }
            catch (Exception ex)
            {
                return Problem(title: ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<UserViewModel>> Update(UserViewModel userViewModel)
        {
            try
            {
                var appUser = userViewModel.ToAppUser();
                await _userManager.UpdateAsync(appUser);               

                return Ok(appUser.ToUserViewModel());
            }
            catch (Exception ex)
            {
                return Problem(title: ex.Message);
            }
        }

        [HttpDelete("delete/{userName}")]
        public async Task<ActionResult> Delete(string userName)
        {
            try
            {
                var appUser = await _userManager.FindByNameAsync(userName);

                var roles = await _userManager.GetRolesAsync(appUser);
                if (roles.Contains(GlobalVarables.Roles.ADMIN))
                    return BadRequest();

                var result = await _userManager.DeleteAsync(appUser);
                return result.Succeeded ? NoContent() : BadRequest();
            }
            catch (Exception ex)
            {
                return Problem(title: ex.Message);
            }
        }

        [HttpPost("changepass")]
        public async Task<ActionResult<AppUser>> ChangePassword(UserChgPassViewModel userViewModel)
        {
            try
            {
                var appUser = await _userManager.FindByNameAsync(userViewModel.UserName);
                var result = await _userManager.ChangePasswordAsync(appUser, userViewModel.CurrentPassword, userViewModel.Password);

                return result.Succeeded ? NoContent() : BadRequest();
            }
            catch (Exception ex)
            {
                return Problem(title: ex.Message);
            }
        }

        [HttpGet("allroles")]
        public ActionResult<string[]> GetRoles() => _roleManager.Roles.Select(r => r.Name).ToArray();

        [HttpPut("changerole")]
        public async Task<ActionResult<UserViewModel>> ChangeRole(UserChgRoleViewModel userViewModel)
        {
            try
            {
                var appUser = await _userManager.FindByNameAsync(userViewModel.UserName);

                await _userManager.RemoveFromRoleAsync(appUser, userViewModel.CurrentRole);
                await _userManager.AddToRoleAsync(appUser, userViewModel.Role);
                await _userManager.UpdateAsync(appUser);

                return Ok(appUser.ToUserViewModel());
            }
            catch (Exception ex)
            {
                return Problem(title: ex.Message);
            }
        }
    }
}
