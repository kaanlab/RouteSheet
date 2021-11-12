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
        ValueTask<Cadet> AddCadet(Cadet cadet);
        ValueTask<Cadet> UpdateCadet(Cadet cadet);
        ValueTask<Cadet> DeleteCadet(Cadet cadet);
        IQueryable<Cadet> GetCadets();
        ValueTask<Cadet> FindCadetById(int id);
    }
}
