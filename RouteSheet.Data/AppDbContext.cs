using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RouteSheet.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Data
{
    public  class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>().Property(e => e.AppUserType).HasConversion<string>();
            modelBuilder.Entity<Lesson>().Property(e => e.Prioriy).HasConversion<string>();

            modelBuilder.Entity<Classroom>().HasMany(e => e.Cadets).WithOne(e => e.Classroom).OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Cadet> Cadets { get; set; }
    }
}
