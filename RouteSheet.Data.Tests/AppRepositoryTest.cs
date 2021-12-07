using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using RouteSheet.Data.Exceptions;
using RouteSheet.Data.Repositories;
using RouteSheet.Shared;
using RouteSheet.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Xunit;

namespace RouteSheet.Data.Tests
{
    public partial class AppRepositoryTest 
    {       
        private AppDbContext AppDbContextInMemory()
        {
            DbContextOptions<AppDbContext> options;
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase("RouteSheetInMemory.db");
            options = builder.Options;
            AppDbContext appDataContext = new AppDbContext(options);
            appDataContext.Database.EnsureDeleted();
            appDataContext.Database.EnsureCreated();

            var dbSetMock = new Mock<DbSet<AppUser>>();
            var dbContextMock = new Mock<AppDbContext>();
            dbContextMock.Setup(s => s.Set<AppUser>()).Returns(dbSetMock.Object);

            var userManagerMock = GetUserManagerMock<AppUser>();
            userManagerMock.Setup(u => u.Users).Returns(SomeAppUsers);
 
            //UserRepository userRepository = new UserRepository(userManagerMock.Object);
            appDataContext.Users.AddRange(userManagerMock.Object.Users);
           // var users = userRepository.GetAppUsers();
            //
            appDataContext.Cadets.AddRange(
                new Cadet[] {
                    new Cadet { Name = "Петров П.П.", Classroom = new Classroom { Name = "7Б" } },
                    new Cadet { Name = "Иванов И.И.", Classroom = new Classroom { Name = "8А" } }
                });

            appDataContext.SaveChanges();

            appDataContext.Lessons.AddRange(
                new Lesson[] {
                    new Lesson { AppUser = appDataContext.Users.First(), Cadet = appDataContext.Cadets.First(), Date =  DateTime.Now, Hour = 1, Prioriy = Priority.Normal, Title = "Робототехника" },
                    new Lesson { AppUser = appDataContext.Users.Last(), Cadet = appDataContext.Cadets.Last(), Date =  DateTime.Now.AddDays(1), Hour = 2, Prioriy = Priority.Normal, Title = "Обследование"} 
                });
            appDataContext.SaveChanges();
            return appDataContext;
        }

        private Mock<UserManager<TIDentityUser>> GetUserManagerMock<TIDentityUser>() where TIDentityUser : IdentityUser
        {
            return new Mock<UserManager<TIDentityUser>>(
                    new Mock<IUserStore<TIDentityUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<TIDentityUser>>().Object,
                    new IUserValidator<TIDentityUser>[0],
                    new IPasswordValidator<TIDentityUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<TIDentityUser>>>().Object);
        }

        private IQueryable<AppUser> SomeAppUsers()
        {
            List<AppUser> appUsers = new List<AppUser>
            {
                 new AppUser { Position = "Администратор", Name="Петров П.П.",UserName = "siteadmin", Email = "petrpku@mil.ru" },
                 new AppUser { Position = "Преподаватель", Name="Иванов И.И.", UserName = "petrpku", Email = "petrpku@mil.ru" }
            };
            return appUsers.AsQueryable();
        }
    }
}
