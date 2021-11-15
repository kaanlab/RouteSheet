using RouteSheet.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Data.Repositories
{
    public partial interface IAppRepository
    {
        ValueTask<Lesson> AddLesson(Lesson lesson);
        ValueTask<Lesson> UpdateLesson(Lesson lesson);
        ValueTask<Lesson> DeleteLesson(Lesson lesson);
        IQueryable<Lesson> GetLessons();
        ValueTask<Lesson> FindLessonById(int id);
    }
}
