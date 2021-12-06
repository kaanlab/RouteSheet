using RouteSheet.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Shared.ViewModels
{
    public class UserAddViewModel
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        //[Compare("Password")]
        public string ConfirmPassword { get; set; }

        public AppUser ToAppUser() => new AppUser()
        {
            Name = this.Name,
            Position = this.Position,
            UserName = this.UserName,
            Email = this.Email
        };

    }
}
