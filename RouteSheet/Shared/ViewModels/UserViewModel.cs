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
        public string Name { get; set; }
        public string Position { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        //public AppUser ToAppUser() => new AppUser()
        //{
        //    Name = this.Name,
        //    Position = this.Position,
        //    UserName = this.UserName,
        //    Email = this.Email            
        //};
    }
}
