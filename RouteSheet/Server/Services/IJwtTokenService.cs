using Microsoft.IdentityModel.Tokens;
using RouteSheet.Shared.Models;
using System.Security.Claims;

namespace RouteSheet.Server.Services
{
    public interface IJwtTokenService
    {
        Task<string> Create(SymmetricSecurityKey key, IList<Claim> claims);
    }
}
