using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        public async ValueTask<Cadet> AddCadet(Cadet cadet)
        {
            try
            {
                if (cadet.Classroom is null)
                    throw new AppRepositoryExeption(new Exception("Classroom is null"));

                EntityEntry<Cadet>? cadetEntry;                
                if (cadet.Classroom?.Id > 0)
                {
                    var classroom = await this.FindClassroomById(cadet.Classroom.Id);
                    cadet.Classroom = classroom;
                }                

                cadetEntry = await _appDbContext.Cadets.AddAsync(cadet);
                await _appDbContext.SaveChangesAsync();
                return cadetEntry.Entity;
            }
            catch (DbUpdateException ex)
            {
                throw new AppRepositoryExeption(ex);
            }
        }

        public async ValueTask<Cadet> UpdateCadet(Cadet cadet)
        {
            try
            {
                var cadetInDb = await this.FindCadetById(cadet.Id);
                if (cadetInDb is null)
                    throw new AppRepositoryExeption(new Exception($"Can't find cadet with id {cadet.Id}"));

                cadetInDb.Name = cadet.Name;

                var classroomInDb = await this.FindClassroomById(cadet.Classroom.Id);
                if (classroomInDb is null)
                    throw new AppRepositoryExeption(new Exception($"Can't find Classroom with id {cadet.Classroom.Id}"));

                cadetInDb.Classroom = classroomInDb;

                var cadetEntry = _appDbContext.Cadets.Update(cadetInDb);
                await _appDbContext.SaveChangesAsync();

                return cadetEntry.Entity;
            }
            catch (DbUpdateException ex)
            {
                throw new AppRepositoryExeption(ex);
            }
        }

        public async ValueTask<Cadet> DeleteCadet(Cadet cadet)
        {
            var cadetEntry = _appDbContext.Cadets.Remove(cadet);
            await _appDbContext.SaveChangesAsync();
            return cadetEntry.Entity;
        }

        public IQueryable<Cadet> GetCadets() => _appDbContext.Cadets.AsQueryable();

        public async ValueTask<Cadet> FindCadetById(int id) => await _appDbContext.Cadets.FindAsync(id);
    }
}
