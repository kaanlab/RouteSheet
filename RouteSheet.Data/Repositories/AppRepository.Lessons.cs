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
        public async ValueTask<Lesson> AddLesson(Lesson lesson)
        {
            try
            {
                if (lesson.AppUser is null)
                    throw new NullReferenceException(nameof(lesson.AppUser));

                if (string.IsNullOrEmpty(lesson.AppUser.Id))
                {
                    var user = await this.FindUserById(lesson.AppUser.Id);
                    if (user is null)
                        throw new NullReferenceException(nameof(user));

                    lesson.AppUser = user;
                }

                if (lesson.Cadet is null)
                    throw new NullReferenceException(nameof(lesson.Cadet));

                if (lesson.Cadet.Id > 0)
                {
                    var cadet = await this.FindCadetById(lesson.Cadet.Id);
                    if (cadet is null)
                        throw new NullReferenceException(nameof(cadet));

                    lesson.Cadet = cadet;
                }

                var lessonEntry = await _appDbContext.Lessons.AddAsync(lesson);
                await _appDbContext.SaveChangesAsync();
                return lessonEntry.Entity;
            }
            catch (NullReferenceException ex)
            {
                throw new AppRepositoryException(ex);
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
        public async ValueTask<Lesson> UpdateLesson(Lesson lesson)
        {
            try
            {
                if (lesson.AppUser is null)
                    throw new NullReferenceException(nameof(lesson.AppUser));

                if (lesson.Cadet is null)
                    throw new NullReferenceException(nameof(lesson.Cadet));

                var lessonInDb = await this.FindLessonById(lesson.Id);
                lessonInDb.Date = lesson.Date;
                lessonInDb.Hour = lesson.Hour;
                lessonInDb.Title = lesson.Title;
                lessonInDb.AppUser = lesson.AppUser;
                lessonInDb.Prioriy = lesson.Prioriy;
                lessonInDb.Cadet = lesson.Cadet;

                var lessonEntry = _appDbContext.Lessons.Update(lessonInDb);
                await _appDbContext.SaveChangesAsync();
                return lessonEntry.Entity;
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

        public async ValueTask<bool> DeleteLesson(int id)
        {
            try
            {
                var lessonInDb = await this.FindLessonById(id);
                if (lessonInDb is null)
                    throw new NullReferenceException(nameof(lessonInDb));

                _appDbContext.Lessons.Remove(lessonInDb);
                var result = await _appDbContext.SaveChangesAsync();
                return result > 0 ? true : false;                
            }
            catch (NullReferenceException ex)
            {
                throw new AppRepositoryException(ex);
            }
        }

        public IQueryable<Lesson> AllLessons() => _appDbContext.Lessons.Include(x => x.Cadet).Include(x => x.AppUser).AsQueryable();

        public async ValueTask<Lesson> FindLessonById(int id) => await _appDbContext.Lessons.FindAsync(id);
    }
}
