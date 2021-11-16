using RouteSheet.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Data.Repositories
{
    public partial interface IAppRepository
    {
        IQueryable<AppUser> GetUsers();
        ValueTask<AppUser> FindUserById(string id);
        AppUser FindUserByUserName(string userName);
    }
}
