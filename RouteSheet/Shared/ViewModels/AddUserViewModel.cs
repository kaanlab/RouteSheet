using RouteSheet.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Shared.ViewModels
{
    public class AddUserViewModel
    {
        public string DisplayName { get; set; }
        public AppUserType AppUserType { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public AppUser ToAppUser() => new AppUser()
        {
            DisplayName = this.DisplayName,
            AppUserType = this.AppUserType,
            UserName = this.UserName,
            Email = this.Email
        };

    }
}
