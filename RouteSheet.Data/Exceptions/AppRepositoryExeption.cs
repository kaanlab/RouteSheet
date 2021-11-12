using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Data.Exceptions
{
    public class AppRepositoryExeption : Exception
    {
        public AppRepositoryExeption(Exception innerException) : base(message: "Проблеммы с базой данных", innerException) { }
    }
}
