using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RouteSheet.Data.Exceptions;
using RouteSheet.Data.Repositories;
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
            builder.UseInMemoryDatabase("RouteSeetInMemory.db");
            options = builder.Options;
            AppDbContext appDataContext = new AppDbContext(options);
            appDataContext.Database.EnsureDeleted();
            appDataContext.Database.EnsureCreated();
            appDataContext.Cadets.AddRange(
                new Cadet[] {
                    new Cadet { Name = "Петров П.П.", Classroom = new Classroom { Name = "7Б" } },
                    new Cadet { Name = "Иванов И.И.", Classroom = new Classroom { Name = "8А" } }
                });
            appDataContext.SaveChanges();
            return appDataContext;
        }
    }
}
