using RouteSheet.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Data.Repositories
{
    public partial class AppRepository
    {
        public IQueryable<AppUser> GetUsers() => _appDbContext.Users.AsQueryable();
        public async ValueTask<AppUser> FindUserById(string id) =>
            await _appDbContext.Users.FindAsync(id);

        public AppUser FindUserByUserName(string userName) =>
            _appDbContext.Users.FirstOrDefault(u => u.UserName == userName);
    }
}
