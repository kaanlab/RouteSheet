using RouteSheet.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Shared.ViewModels
{
    public class CadetViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        //
        [Required]
        public ClassroomViewModel Classroom { get; set; }

        public Cadet ToCadetModel() => new Cadet
        {
            Id = Id,
            Name = Name,
            Classroom = Classroom.ToClassroomModel()

        };
    }
}
