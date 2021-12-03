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
        public UsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpGet("all")]
        public ActionResult<IList<UserViewModel>> GetUsers()
        {
            var users = _userManager.Users.AsQueryable();
            var usersViewModels = users.Select(x => x.ToUserViewModel());

            return Ok(usersViewModels.ToList());
        }

        [HttpPost("add")]
        public async Task<ActionResult<UserViewModel>> Add(AddUserViewModel userViewModel)
        {
            try
            {
                var appUser = userViewModel.ToAppUser();
                await _userManager.CreateAsync(appUser, userViewModel.Password);
                await _userManager.AddToRoleAsync(appUser, GlobalVarables.Roles.USER);

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
        public async Task<ActionResult<AppUser>> ChangePassword(ChgPassUserViewModel userModelViewModel)
        {
            try
            {
                var appUser = await _userManager.FindByNameAsync(userModelViewModel.UserName);
                var result = await _userManager.ChangePasswordAsync(appUser, userModelViewModel.CurrentPassword, userModelViewModel.Password);

                return result.Succeeded ? NoContent() : BadRequest();
            }
            catch (Exception ex)
            {
                return Problem(title: ex.Message);
            }
        }
    }
}
