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
        ValueTask<Classroom> AddClassroom(Classroom classroom);
        ValueTask<Classroom> UpdateClassroom(Classroom classroom);
        ValueTask<bool> DeleteClassroom(int id);
        IQueryable<Classroom> AllClassrooms();
        ValueTask<Classroom> FindClassroomById(int id);
    }
}
