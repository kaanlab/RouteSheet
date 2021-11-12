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
        ValueTask<Classroom> AddClassroom(Classroom cadet);
        ValueTask<Classroom> UpdateClassroom(Classroom cadet);
        ValueTask<Classroom> DeleteClassroom(Classroom cadet);
        IQueryable<Classroom> GetClassroom();
        ValueTask<Classroom> FindClassroomById(int id);
    }
}
