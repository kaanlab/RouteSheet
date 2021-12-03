using RouteSheet.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Shared.ViewModels
{
    public class UserInRouteSheetViewModel
    {
        public string Id { get; set; }
        public AppUserType AppUserType { get; set; }
        public string DisplayName { get; set; }
    }
}
