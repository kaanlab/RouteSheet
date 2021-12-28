using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RouteSheet.Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RouteSheet.Server.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        public async Task<string> Create(SymmetricSecurityKey key, IList<Claim> claims)
        {
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
