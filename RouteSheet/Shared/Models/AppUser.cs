using Microsoft.AspNetCore.Identity;
using RouteSheet.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Shared.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Position { get; set; }

        //
        public IEnumerable<Lesson> Lessons { get; set; }

        public UserViewModel ToUserViewModel() => new UserViewModel()
        {
            Name = this.Name,
            UserName = this.UserName,
            Email = this.Email
        };
    }
}
