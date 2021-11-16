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
            catch (ArgumentNullException ex)
            {
                throw new AppRepositoryExeption(ex);
            }
            catch (DbUpdateException ex)
            {
                throw new AppRepositoryExeption(ex);
            }
        }
        public async ValueTask<Lesson> UpdateLesson(Lesson lesson)
        {
            try
            {
                var lessonInDb = await this.FindLessonById(lesson.Id);
                lessonInDb.Date = lesson.Date;
                lessonInDb.Hour = lesson.Hour;
                lessonInDb.Title = lesson.Title;
                lessonInDb.AppUser = lesson.AppUser;
                lessonInDb.Prioriy = lesson.Prioriy;

                var lessonEntry = _appDbContext.Lessons.Update(lessonInDb);
                await _appDbContext.SaveChangesAsync();
                return lessonEntry.Entity;
            }
            catch (ArgumentNullException ex)
            {
                throw new AppRepositoryExeption(ex);
            }
            catch (NullReferenceException ex)
            {
                throw new AppRepositoryExeption(ex);
            }
            catch (DbUpdateException ex)
            {
                throw new AppRepositoryExeption(ex);
            }
        }

        public async ValueTask<Lesson> DeleteLesson(Lesson lesson)
        {
            try
            {
                var lessonInDb = await this.FindLessonById(lesson.Id);
                var lessonEntry = _appDbContext.Lessons.Remove(lessonInDb);
                await _appDbContext.SaveChangesAsync();
                return lessonEntry.Entity;
            }
            catch (ArgumentNullException ex)
            {
                throw new AppRepositoryExeption(ex);
            }
            catch (NullReferenceException ex)
            {
                throw new AppRepositoryExeption(ex);
            }
        }

        public IQueryable<Lesson> GetLessons() => _appDbContext.Lessons.AsQueryable();

        public async ValueTask<Lesson> FindLessonById(int id) => await _appDbContext.Lessons.FindAsync(id);
    }
}
