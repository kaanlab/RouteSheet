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
    public enum AppUserType
    {
        [Display(Name ="Учитель")]
        Teacher,
        [Display(Name ="Медик")]
        Medic
    }
    
    public class AppUser : IdentityUser
    {
        
        public AppUserType AppUserType { get; set; }
        public string DisplayName { get; set; }

        //
        public IEnumerable<Lesson> Lessons { get; set; }

        public UserViewModel ToUserViewModel() => new UserViewModel()
        {
            DisplayName = this.DisplayName,
            AppUserType = this.AppUserType,
            UserName = this.UserName,
            Email = this.Email
        };
    }
}
