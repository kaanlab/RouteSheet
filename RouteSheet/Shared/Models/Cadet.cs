using RouteSheet.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Shared.Models
{
    public class Cadet
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        

        public Classroom? Classroom { get; set; }

        public IEnumerable<Lesson>? Lessons { get; set; }

        public CadetViewModel ToCadetViewModel() => new CadetViewModel
        {
            Id = Id,
            Name = Name,
            Classroom = Classroom is null ? new ClassroomViewModel() : Classroom.ToClassroomViewModel()
        };
    }
}
