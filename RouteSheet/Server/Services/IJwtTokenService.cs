using Microsoft.IdentityModel.Tokens;
using RouteSheet.Shared.Models;

namespace RouteSheet.Server.Services
{
    public interface IJwtTokenService
    {
        Task<string> Create(AppUser appUser, SymmetricSecurityKey key);
    }
}
