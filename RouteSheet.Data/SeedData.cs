using Microsoft.AspNetCore.Identity;
using RouteSheet.Shared;
using RouteSheet.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Data
{
    public class SeedData
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedData(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task AddUsers()
        {
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole(GlobalVarables.Roles.ADMIN));
                await _roleManager.CreateAsync(new IdentityRole(GlobalVarables.Roles.TEACHER));
                await _roleManager.CreateAsync(new IdentityRole(GlobalVarables.Roles.MEDIC));                 
            }

            if (!_userManager.Users.Any())
            {
                var admin = new AppUser { Name = "Иванов И.И.", Position = "Администратор", UserName = "siteadmin", Email = "petrpku@mil.ru",  };
                await _userManager.CreateAsync(admin, "1Password!");
                await _userManager.AddToRoleAsync(admin, GlobalVarables.Roles.ADMIN);

                var teacher = new AppUser { Name = "Петров П.П.", Position = "Учитель", UserName = "teacher", Email = "petrpku@mil.ru", };
                await _userManager.CreateAsync(teacher, "1Password!");
                await _userManager.AddToRoleAsync(teacher, GlobalVarables.Roles.TEACHER);

                var medic = new AppUser { Name = "Сидоров С.С.", Position = "Медик", UserName = "medic", Email = "petrpku@mil.ru", };
                await _userManager.CreateAsync(medic, "1Password!");
                await _userManager.AddToRoleAsync(medic, GlobalVarables.Roles.MEDIC);
            }
        }
    }
}
