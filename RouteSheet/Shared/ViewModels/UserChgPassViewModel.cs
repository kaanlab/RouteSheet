using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Shared.ViewModels
{
    public class UserChgPassViewModel
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
