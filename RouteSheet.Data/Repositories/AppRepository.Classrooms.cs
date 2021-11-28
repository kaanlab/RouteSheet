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
            catch (ArgumentNullException ex)
            {
                throw new AppRepositoryException(ex);
            }
            catch (DbUpdateException ex)
            {
                throw new AppRepositoryException(ex);
            }
        }

        public async ValueTask<Classroom> UpdateClassroom(Classroom classroom)
        {
            try
            {
                var classroomInDb = await this.FindClassroomById(classroom.Id);
                classroomInDb.Name = classroom.Name;

                var classroomEntry = _appDbContext.Classrooms.Update(classroomInDb);
                await _appDbContext.SaveChangesAsync();
                return classroomEntry.Entity;
            }
            catch (ArgumentNullException ex)
            {
                throw new AppRepositoryException(ex);
            }
            catch (NullReferenceException ex)
            {
                throw new AppRepositoryException(ex);
            }
            catch (DbUpdateException ex)
            {
                throw new AppRepositoryException(ex);
            }
        }

        public async ValueTask<bool> DeleteClassroom(int id)
        {
            try
            {
                var classroomInDb = await this.FindClassroomById(id);
                if (classroomInDb is null)
                    throw new NullReferenceException(nameof(classroomInDb));

                _appDbContext.Classrooms.Remove(classroomInDb);
                var result = await _appDbContext.SaveChangesAsync();
                return result > 0 ? true : false;
            }

            catch (NullReferenceException ex)
            {
                throw new AppRepositoryException(ex);
            }
        }

        public IQueryable<Classroom> AllClassrooms() => _appDbContext.Classrooms.AsQueryable();

        public async ValueTask<Classroom> FindClassroomById(int id) => await _appDbContext.Classrooms.FindAsync(id);
    }
}
