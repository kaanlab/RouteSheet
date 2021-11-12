using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Shared.Models
{
    public class Lesson
    {
        public int Id { get; set; }

        public DateTime? Date { get; set; }

        public int Hour { get; set; }

        public string Title { get; set; }

        public string TeacherName { get; set; }

        public int Prioriy { get; set; }
    }
}
