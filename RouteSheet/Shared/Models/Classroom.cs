using RouteSheet.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RouteSheet.Shared.Models
{
    public class Classroom
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        //
        public IEnumerable<Cadet>? Cadets { get; set; }

        public ClassroomViewModel ToClassroomViewModel() => new ClassroomViewModel
        {
            Id = Id,
            Name = Name
        };
    }
}
