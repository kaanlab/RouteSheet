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
                ArgumentNullException.ThrowIfNull(cadet);

                if(cadet.Classroom is null)
                    throw new NullReferenceException(nameof(cadet.Classroom));

                if (cadet.Classroom.Id > 0)
                {
                    var classroom = await this.FindClassroomById(cadet.Classroom.Id);
                    if (classroom is null)
                        throw new NullReferenceException(nameof(classroom));

                    cadet.Classroom = classroom;
                }                

                var cadetEntry = await _appDbContext.Cadets.AddAsync(cadet);
                await _appDbContext.SaveChangesAsync();
                return cadetEntry.Entity;
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

        public async ValueTask<Cadet> UpdateCadet(Cadet cadet)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(cadet);

                var cadetInDb = await this.FindCadetById(cadet.Id); 
                cadetInDb.Name = cadet.Name;

                if (cadetInDb.Classroom is not null && cadet.Classroom.Id > 0)
                {
                    var classroom = await this.FindClassroomById(cadet.Classroom.Id);
                    if (classroom is null)
                        throw new NullReferenceException(nameof(classroom));

                    cadetInDb.Classroom = classroom;
                }

                var cadetEntry = _appDbContext.Cadets.Update(cadetInDb);
                await _appDbContext.SaveChangesAsync();

                return cadetEntry.Entity;
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

        public async ValueTask<bool> DeleteCadet(int id)
        {
            try
            {
                var cadetInDb = await this.FindCadetById(id);
                if (cadetInDb is null)
                    throw new NullReferenceException(nameof(cadetInDb));

                _appDbContext.Cadets.Remove(cadetInDb);
                var result = await _appDbContext.SaveChangesAsync();
                return result > 0 ? true : false; 
            }
            catch(NullReferenceException ex)
            {
                throw new AppRepositoryException(ex);
            }
        }

        public IQueryable<Cadet> AllCadets() => _appDbContext.Cadets.AsQueryable();

        public async ValueTask<Cadet> FindCadetById(int id) => await _appDbContext.Cadets.FindAsync(id);    
    }
}
