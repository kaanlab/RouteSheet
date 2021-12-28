using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RouteSheet.Server.Services;
using RouteSheet.Shared;
using RouteSheet.Shared.Models;
using RouteSheet.Shared.ViewModels;
using System.Security.Claims;

namespace RouteSheet.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResultViewModel>> Login(LoginViewModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.Login) || string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest();
            }

            var user = await _userManager.FindByNameAsync(loginModel.Login);
            if (user is null)
            {
                return Unauthorized(new LoginResultViewModel { Successful = false, Error = "Неверное имя пользователя или пароль!" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
            if (result.Succeeded)
            {
                var claims = await GetClaims(user);
                
                return new LoginResultViewModel
                {
                    Successful = true,
                    Token = await _jwtTokenService.Create(GlobalVarables.KEY, claims)
                };
            }

            return Unauthorized(new LoginResultViewModel { Successful = false, Error = "Неверное имя пользователя или пароль!" });
        }

        private async Task<IList<Claim>> GetClaims(AppUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, user.Position)
            };
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
