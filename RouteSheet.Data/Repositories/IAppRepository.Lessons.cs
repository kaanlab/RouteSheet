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
        ValueTask<int> DeleteLesson(int id);
        IQueryable<Lesson> AllLessons();
        ValueTask<Lesson> FindLessonById(int id);
    }
}
