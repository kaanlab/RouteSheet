using RouteSheet.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Shared.ViewModels
{
    public class UserViewModel
    {
        public AppUserType AppUserType { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public AppUser ToAppUser() => new AppUser()
        {
            DisplayName = this.DisplayName,
            AppUserType = this.AppUserType,
            UserName = this.UserName,
            Email = this.Email
        };
    }
}
