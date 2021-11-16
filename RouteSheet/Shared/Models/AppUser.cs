using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Shared.Models
{
    public enum AppUserType
    {
        Teacher,
        Medic
    }
    
    public class AppUser : IdentityUser
    {
        public AppUserType AppUserType { get; set; }
        public string DisplayName { get; set; }

        //
        public IEnumerable<Lesson> Lessons { get; set; }
    }
}
