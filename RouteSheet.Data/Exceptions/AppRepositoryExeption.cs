using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Data.Exceptions
{
    public class AppRepositoryExeption : Exception
    {
        public AppRepositoryExeption(Exception innerException) : base(message: "Data layer problems, see details for more info", innerException) { }
    }
}
