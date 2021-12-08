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

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }

        //public UserViewModel ToUserViewModel() => new UserViewModel()
        //{
        //    Name = this.Name,
        //    Position = this.Position,
        //    UserName = this.UserName,
        //    Email = this.Email
        //};
    }
}
