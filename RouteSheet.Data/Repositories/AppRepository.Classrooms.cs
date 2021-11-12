using Microsoft.EntityFrameworkCore;
using RouteSheet.Data.Exceptions;
using RouteSheet.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Data.Repositories
{
    public partial class AppRepository
    {
        public async ValueTask<Classroom> AddClassroom(Classroom classroom)
        {
            try
            {
                var classroomEntry = await _appDbContext.Classrooms.AddAsync(classroom);
                await _appDbContext.SaveChangesAsync();
                return classroomEntry.Entity;
            }
            catch (DbUpdateException ex)
            {
                throw new AppRepositoryExeption(ex);
            }
        }

        public async ValueTask<Classroom> UpdateClassroom(Classroom classroom)
        {
            var classroomEntry = _appDbContext.Classrooms.Update(classroom);
            await _appDbContext.SaveChangesAsync();
            return classroomEntry.Entity;
        }

        public async ValueTask<Classroom> DeleteClassroom(Classroom classroom)
        {
            var classroomEntry = _appDbContext.Classrooms.Remove(classroom);
            await _appDbContext.SaveChangesAsync();
            return classroomEntry.Entity;
        }

        public IQueryable<Classroom> GetClassroom() => _appDbContext.Classrooms.AsQueryable();

        public async ValueTask<Classroom> FindClassroomById(int id) => await _appDbContext.Classrooms.FindAsync(id);
    }
}
