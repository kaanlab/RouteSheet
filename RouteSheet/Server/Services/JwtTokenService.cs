using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RouteSheet.Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RouteSheet.Server.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly UserManager<AppUser> _userManager;

        public JwtTokenService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> Create(AppUser appUser, SymmetricSecurityKey key)
        {
            var roles = await _userManager.GetRolesAsync(appUser);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
                new Claim(ClaimTypes.Name, appUser.UserName),
                new Claim(ClaimTypes.GivenName, appUser.Position)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
