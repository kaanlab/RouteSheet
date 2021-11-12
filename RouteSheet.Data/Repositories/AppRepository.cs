using Microsoft.AspNetCore.Identity;
using RouteSheet.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Data.Repositories
{
    public partial class AppRepository : IAppRepository
    {
        private readonly AppDbContext _appDbContext;
        public AppRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

       
    }
}
